using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DAL.Entities.FileEngine
{
	/// <summary>
	/// 文件上传状态
	/// </summary>
	public class UploadCache : BaseEntity
	{
		/// <summary>
		/// 文件上传列表
		/// </summary>
		public virtual List<FileUploadStatus> FileStatus { get; set; }
	}

	public class FileUploadStatus : BaseEntity
	{
		public virtual UserFileInfo FileInfo { get; set; }
		public long Current { get; set; }
		public long Total { get; set; }

		/// <summary>
		/// 最后一次更新时间，超过一定时间则自动删除
		/// </summary>
		public DateTime LastUpdate { get; set; }
	}
}