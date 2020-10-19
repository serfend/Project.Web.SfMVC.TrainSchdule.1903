using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.File;
using Castle.Core.Internal;
using DAL.Data;
using DAL.Entities.FileEngine;
using DAL.QueryModel;
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
		private readonly ICurrentUserService currentUserService;
		private const string upload_file_cache = "upload_file_cache";

		/// <summary>
		/// 每上传128KB进行一次记录（需要额外设置时间判断）
		/// </summary>
		private const int upload_bufsize = 1024 * 128;

		public FileServices(ApplicationDbContext context, IHttpContextAccessor httpContext, ICurrentUserService currentUserService)
		{
			this.context = context;
			this.httpContext = httpContext;
			this.currentUserService = currentUserService;
		}

		public UserFile Download(Guid id) => context.UserFiles.Where(f => f.Id == id).FirstOrDefault();

		public UserFileInfo FileInfo(Guid id) => context.UserFileInfosDb.Where(f => f.Id == id).FirstOrDefault();

		public Tuple<IQueryable<UserFileInfo>, int> FolderFiles(string filepath, QueryByPage pages)
		{
			var node = GetNode(filepath, false, null);
			var list = context.UserFileInfos.AsQueryable();

			if (node == null)
				list = list.Where(f => f.Parent == null).Where(f => f.Path == filepath);
			else
				list = list.Where(f => f.Parent.Id == node.Id);
			list = list.Where(f => f.Name != null).OrderByDescending(f => f.Create);
			return list.SplitPage<UserFileInfo>(pages);
		}

		public Tuple<IEnumerable<string>, int> Folders(string filepath, QueryByPage pages)
		{
			var node = GetNode(filepath, false, null);
			var list = context.UserFileInfos.AsQueryable();
			if (node == null)
				list = list.Where(f => f.Parent == null);
			else
				list = list.Where(f => f.Parent.Id == node.Id);
			list = list.Where(f => f.Name == null).OrderByDescending(f => f.Create);
			var result = list.SplitPage(pages);
			return new Tuple<IEnumerable<string>, int>(result.Item1.Select(c => c.Path), result.Item2);
		}

		public UserFileInfo GetNode(string path, bool mkdWhenNotExist = false, UserFileInfo current = null)
		{
			if (path == null) return current;
			var exactPath = path.Trim('/');
			if (path.IsNullOrEmpty()) return null;
			var list = current == null ?
				context.UserFileInfosDb.Where(f => f.Parent == null)
				: context.UserFileInfosDb.Where(f => f.Parent != null).Where(f => f.Parent.Id == current.Id);
			var paths = exactPath.Split('/');
			var next = list.Where(f => f.Path == paths[0]).FirstOrDefault();
			if (next == null && mkdWhenNotExist)
			{
				next = new UserFileInfo()
				{
					Path = paths[0],
					Parent = current,
					Create = DateTime.Now,
				};
				context.UserFileInfos.Add(next);
				context.SaveChanges();
			}
			if (next == null) return null;
			if (paths.Length > 1) return GetNode(exactPath.Substring(paths[0].Length + 1), mkdWhenNotExist, next);
			return next;
		}

		public UserFileInfo LoadFromCurrentPath(string filename, UserFileInfo current)
		{
			var result = context.UserFileInfos.AsQueryable();
			if (current == null)
				result = result.Where(c => c.Parent == null);
			else
				result = result.Where(c => c.Parent != null && c.Parent.Id == current.Id);
			result = result.Where(c => c.Name == filename);
			return result.FirstOrDefault();
		}

		public UserFileInfo Load(string path, string filename)
		{
			var node = GetNode(path, false);
			// 兼容老版本文件（文件自身带Path）
			var result = node == null ?
				null : LoadFromCurrentPath(filename, node);
			if (result == null) result = context.UserFileInfosDb.Where(f => f.Parent == null).Where(f => f.Path == path).Where(f => f.Name == filename).FirstOrDefault();
			return result;
		}

		private static readonly object fileLock = new object();

		public async Task<UserFileInfo> Upload(IFormFile file, string path, string filename, Guid uploadStatusId, Guid clientKey)
		{
			if (file == null) return null;
			UserFile f = null;
			UserFileInfo fi = null;
			filename = filename.IsNullOrEmpty() ? file.FileName : filename;
			var currentNode = GetNode(path, true, null);
			if (uploadStatusId == Guid.Empty)
			{
				// 判断文件是否已存在，若已存在则先删除
				lock (fileLock)
				{
					fi = LoadFromCurrentPath(filename, currentNode);

					if (fi != null)
					{
						f = context.UserFiles.Where(st => st.Id == fi.Id).FirstOrDefault();
						if (!fi.IsRemoved && fi.ClientKey != clientKey) throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.Default);

						var uploadingFile = context.FileUploadStatuses.Where(st => st.FileInfo.Id == fi.Id).FirstOrDefault();
						if (uploadingFile != null) context.FileUploadStatuses.Remove(uploadingFile);
						context.UserFileInfos.Remove(fi);
						context.SaveChanges();
					}
					else
					{
						fi = new UserFileInfo()
						{
							Id = Guid.NewGuid(),
							Name = filename,
							Parent = currentNode,
							Path = null,
							Create = DateTime.Now,
						};
					}
					if (f == null)
					{
						f = new UserFile()
						{
							Id = fi.Id
						};
						context.UserFiles.Add(f);
					}
					fi.LastModefy = DateTime.Now;
					fi.Length = file.Length;
					fi.FromClient = httpContext.HttpContext.Connection.RemoteIpAddress.ToString();
					fi.ClientKey = clientKey == Guid.Empty ? Guid.NewGuid() : clientKey;
					fi.CreateBy = currentUserService?.CurrentUser;
					if (fi != null) context.UserFileInfos.Add(fi);
					// by serfend @ 202010092054 保存新的文件夹状态，防止多个文件夹被创建
					context.SaveChangesAsync().ConfigureAwait(true);
				}
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
				if (lastStatus == null) throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.CacheIsInvalid);
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
			await context.SaveChangesAsync().ConfigureAwait(true);

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
				await context.SaveChangesAsync().ConfigureAwait(true);
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

		/// <summary>
		/// 自动删除失效的文件上传状态
		/// </summary>
		public void RemoveTimeoutUploadStatus()
		{
			var outofDateStatus = DateTime.Now.AddMinutes(30);
			var list = context.FileUploadStatuses.Where(s => s.LastUpdate < outofDateStatus);
			context.FileUploadStatuses.RemoveRange(list);
			context.SaveChanges();
		}

		public bool Remove(UserFileInfo file)
		{
			if (file == null) return false;
			var data = context.UserFiles.Where(f => f.Id == file.Id).FirstOrDefault();
			if (data == null) return false;
			data.Remove();
			context.UserFiles.Update(data);
			file.Remove();
			context.UserFileInfos.Update(file);
			context.SaveChanges();
			return true;
		}
	}
}