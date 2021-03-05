using BLL.Interfaces.File;
using Castle.Core.Internal;
using DAL.Entities.FileEngine;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Static;

namespace TrainSchdule.Extensions.Common
{
	/// <summary>
	/// 文件处理
	/// </summary>
	public static class FileExtensions
	{
		private const string fileDownloadPathById = "/file/download?fileid=";
		private const string fileDownloadPathByStatic = "/file/staticFile/";
		private const string fileDownloadPathByPath = "/file/fromPath";

		/// <summary>
		/// 下载方式
		/// </summary>
		public enum DownloadType
		{
			/// <summary>
			/// 通过id
			/// </summary>
			ById,

			/// <summary>
			/// 通过直链
			/// </summary>
			ByStatic,

			/// <summary>
			/// 通过文件路径
			/// </summary>
			ByPath
		}

		/// <summary>
		/// 下载链接
		/// </summary>
		/// <param name="file"></param>
		/// <param name="downloadType"></param>
		/// <returns></returns>

		public static string DownloadUrl(this UserFileInfo file, DownloadType downloadType = DownloadType.ByPath)
		{
			switch (downloadType)
			{
				case DownloadType.ByPath:

					return $"{fileDownloadPathByPath}?path={file.FullPath()}&filename={file?.Name}";

				case DownloadType.ById:
					return $"{fileDownloadPathById}{file?.Id}";

				case DownloadType.ByStatic:
					return $"{fileDownloadPathByStatic}{file?.Id}";

				default:
					throw new ArgumentOutOfRangeException(nameof(downloadType));
			}
		}

		/// <summary>
		/// 获取绝对路径
		/// </summary>
		/// <param name="file"></param>
		/// <param name="nowPath"></param>
		/// <returns></returns>
		public static string FullPath(this UserFileInfo file, string nowPath = "")
		{
			if (file == null) return nowPath;
			var currentNode = file.Path.IsNullOrEmpty() ? null : $"/{file.Path}";
			var nodePath = nowPath.IsNullOrEmpty() ? null : $"/{nowPath}";
			return file == null ? nowPath : $"{file.Parent.FullPath()}{currentNode}{nodePath}";
		}
        /// <summary>
        /// 快捷上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileServices"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="removeTime"></param>
        /// <returns></returns>
        public static async Task<FileReturnViewModel> UploadToDb(this FormFile file,IFileServices fileServices,string path, string fileName,TimeSpan? removeTime=null)
        {
			var tmpFile = await fileServices.Upload(file, path, fileName, Guid.Empty, Guid.Empty).ConfigureAwait(true);
			removeTime ??= TimeSpan.FromDays(7);
			return new FileReturnViewModel()
			{
				Data = new FileReturnDataModel()
				{
					FileName = fileName,
					RequestUrl = tmpFile.DownloadUrl(FileExtensions.DownloadType.ByPath),
					ValidStamp = DateTime.Now.Add(removeTime.Value),
					Length = tmpFile.Length
				}
			};
		}
	}
}