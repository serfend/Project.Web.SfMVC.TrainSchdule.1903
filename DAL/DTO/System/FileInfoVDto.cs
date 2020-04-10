using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTO.System
{
	public class FileInfoVDto
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
		public DateTime LastModefy { get; set; }

		/// <summary>
		/// 路径
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// 文件来源ip
		/// </summary>
		public string FromClient { get; set; }

		/// <summary>
		/// 文件是否存在，在创建文件时置为True
		/// </summary>
		public bool Exist { get; set; }
	}
}