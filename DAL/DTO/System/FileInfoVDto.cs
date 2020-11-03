using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTO.System
{
	public class FileInfoVDto : BaseEntityGuid
	{
		/// <summary>
		/// 文件名
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 文件大小
		/// </summary>
		public long Length { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime Create { get; set; }

		/// <summary>
		/// 上次修改时间
		/// </summary>
		public DateTime LastModify { get; set; }

		/// <summary>
		/// 路径
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// 文件来源ip
		/// </summary>
		public string FromClient { get; set; }
	}
}