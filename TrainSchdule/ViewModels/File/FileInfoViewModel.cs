using BLL.Helpers;
using DAL.Entities.FileEngine;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.File
{
	/// <summary>
	/// 用户上传的文件信息
	/// </summary>
	public class FileInfoViewModel : ApiResult
	{
		public FileInfoDataModel Data { get; set; }
	}

	public class FileInfoDataModel
	{
		public UserFileInfo File { get; set; }
	}

	/// <summary>
	/// 文件上传
	/// </summary>
	public class FileUploadViewModel
	{
		/// <summary>
		/// 上传的文件
		/// </summary>
		public IFormFile File { get; set; }

		/// <summary>
		/// 文件名称
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// 断线续传时需要提供id
		/// </summary>
		public string ResumeUploadId { get; set; }

		/// <summary>
		/// 文件路径
		/// </summary>
		public string FilePath { get; set; }
	}
}