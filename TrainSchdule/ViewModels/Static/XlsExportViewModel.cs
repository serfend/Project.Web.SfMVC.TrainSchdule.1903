﻿using BLL.Helpers;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Static
{
	/// <summary>
	/// 导出申请到指定模板
	/// </summary>
	public class AppliesExportDataModel
	{
		/// <summary>
		/// 模板名称
		/// </summary>
		public string Templete { get; set; }
		/// <summary>
		/// 导出申请的条件
		/// </summary>
		public QueryApplyDataModel Query { get; set; }
	}
	/// <summary>
	/// 文件导出
	/// </summary>
	public class FileReturnViewModel : ApiResult
	{
		public FileReturnDataModel Data { get; set; }
	}
	public class FileReturnDataModel
	{
		/// <summary>
		/// 文件名
		/// </summary>
		public string FileName { get; set; }
		/// <summary>
		/// 临时下载链接
		/// </summary>
		public string RequestUrl { get; set; }
		/// <summary>
		/// 有效期至
		/// </summary>
		public long ValidStamp { get; set; }
		/// <summary>
		/// 长度
		/// </summary>
		public long Length { get; set; }
	}
}
