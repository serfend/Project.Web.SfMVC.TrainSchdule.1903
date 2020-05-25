using BLL.Interfaces.File;
using Castle.Core.Internal;
using DAL.Entities.FileEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
			var currentNode = file.Path.IsNullOrEmpty() ? null : $"{file.Path}/";
			return file == null ? nowPath : $"{FullPath(file.Parent)}{currentNode}{nowPath}";
		}
	}
}