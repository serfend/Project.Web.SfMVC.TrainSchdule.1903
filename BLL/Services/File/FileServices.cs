using BLL.Interfaces.File;
using Castle.Core.Internal;
using DAL.Data;
using DAL.Entities.FileEngine;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.File
{
	public class FileServices : IFileServices
	{
		private readonly ApplicationDbContext context;
		private readonly IHttpContextAccessor httpContext;
		private const string upload_file_cache = "upload_file_cache";

		/// <summary>
		/// 每上传128KB进行一次记录（需要额外设置时间判断）
		/// </summary>
		private const int upload_bufsize = 1024 * 128;

		public FileServices(ApplicationDbContext context, IHttpContextAccessor httpContext)
		{
			this.context = context;
			this.httpContext = httpContext;
		}

		public UserFile Download(Guid id) => context.UserFiles.Find(id);

		public UserFileInfo FileInfo(Guid id) => context.UserFileInfos.Find(id);

		public UserFileInfo Load(string path, string filename) => context.UserFileInfos.Where(c => c.Path == path).Where(c => c.Name == filename).FirstOrDefault();

		public async Task<UserFileInfo> Upload(IFormFile file, string path, string filename, Guid uploadStatusId)
		{
			if (file == null) return null;
			UserFile f = null;
			UserFileInfo fi = null;
			filename = filename.IsNullOrEmpty() ? file.FileName : filename;
			if (uploadStatusId == Guid.Empty)
			{
				f = new UserFile();
				context.UserFiles.Add(f);
				// 判断文件是否已存在，若已存在则先删除
				fi = Load(path, filename);
				if (fi != null)
				{
					var prevFile = context.UserFiles.Find(fi.Id);
					var uploadingFile = context.FileUploadStatuses.Where(st => st.FileInfo.Id == fi.Id).FirstOrDefault();
					if (uploadingFile != null) context.FileUploadStatuses.Remove(uploadingFile);
					context.UserFiles.Remove(prevFile);
					context.UserFileInfos.Remove(fi);
					await context.SaveChangesAsync().ConfigureAwait(true);
				}
				fi = new UserFileInfo()
				{
					Id = f.Id,
					Name = filename,
					Path = path,
					Create = DateTime.Now,
					Length = file.Length
				};
				context.UserFileInfos.Add(fi);
			}
			using (var inputStream = file.OpenReadStream())
			{
				await UploadInner(fi, f, inputStream, uploadStatusId).ConfigureAwait(true);
			}
			await context.SaveChangesAsync().ConfigureAwait(true);
			return fi;
		}

		private async Task UploadInner(UserFileInfo fileInfo, UserFile file, Stream sr, Guid uploadStatusId)
		{
			var uploadStatus = Stauts();
			if (uploadStatusId != Guid.Empty)
			{
				var lastStatus = uploadStatus.FileStatus.Where(s => s.Id == uploadStatusId).FirstOrDefault();
				if (lastStatus == null) throw new Exception("断点续传失败，因为缓存已失效");
				fileInfo = lastStatus.FileInfo;
				file = context.UserFiles.Find(uploadStatusId);
			}
			var f = new FileUploadStatus()
			{
				FileInfo = fileInfo,
				Total = fileInfo.Length,
				LastUpdate = DateTime.Now
			};
			file.Data = new byte[fileInfo.Length];

			uploadStatus.FileStatus.Add(f);
			await context.SaveChangesAsync().ConfigureAwait(false);

			try
			{
				while (f.Current < f.Total)
				{
					var bufSize = f.Total - upload_bufsize - f.Current > 0 ? upload_bufsize : f.Total - f.Current;
					await sr.ReadAsync(file.Data, (int)f.Current, (int)bufSize).ConfigureAwait(true);
					f.Current += bufSize;
					// 只有在时间达到10秒时才更新进度，减少服务器消耗
					if (DateTime.Now.Subtract(f.LastUpdate).TotalSeconds > 10)
					{
						f.LastUpdate = DateTime.Now;
						context.FileUploadStatuses.Update(f);
						await context.SaveChangesAsync().ConfigureAwait(true);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"UplloadInner.Exception:{ex.Message}");
				throw ex;
			}
			finally
			{
				context.UserFiles.Update(file);
				context.FileUploadStatuses.Update(f);
				await context.SaveChangesAsync().ConfigureAwait(false);
			}
		}

		public UploadCache Stauts()
		{
			// 获取文件下载列表
			var status = httpContext.HttpContext.Session.Get(upload_file_cache);
			if (status == null)
			{
				var s = new UploadCache()
				{
					FileStatus = new List<FileUploadStatus>()
				};
				context.UploadCaches.Add(s);
				httpContext.HttpContext.Session.Set(upload_file_cache, Encoding.UTF8.GetBytes(s.Id.ToString()));
				context.SaveChanges();
				return s;
			}
			var id = Encoding.UTF8.GetString(status);
			var item = context.UploadCaches.Find(Guid.Parse(id));
			if (item == null)
			{
				httpContext.HttpContext.Session.Remove(upload_file_cache);
				return Stauts();
			}
			return item;
		}

		public void RemoveTimeoutUploadStatus()
		{
			var list = context.FileUploadStatuses.Where(s => DateTime.Now.Subtract(s.LastUpdate).TotalMilliseconds > 30).ToList();
			context.FileUploadStatuses.RemoveRange(list);
			context.SaveChanges();
		}
	}
}