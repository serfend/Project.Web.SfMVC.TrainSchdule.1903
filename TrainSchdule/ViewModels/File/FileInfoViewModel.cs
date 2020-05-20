using BLL.Helpers;
using DAL.DTO.System;
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
	{/// <summary>
	 ///
	 /// </summary>
		public FileInfoDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class FoldersViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public FoldersDataModel Data { get; set; }
	}

	/// <summary>
	/// 文件列表信息
	/// </summary>
	public class FileInfosViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public FileInfosDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class FileInfoDataModel
	{
		/// <summary>
		///
		/// </summary>
		public FileInfoVDto File { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class FileInfosDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<FileInfoVDto> Files { get; set; }

		/// <summary>
		///
		/// </summary>

		public int TotalCount { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class FoldersDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<string> Folders { get; set; }

		/// <summary>
		///
		/// </summary>

		public int TotalCount { get; set; }
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

		/// <summary>
		/// 当文件已存在时，需要传入原文件的Key以操作，防止越权
		/// </summary>
		public string ClientKey { get; set; }
	}
}