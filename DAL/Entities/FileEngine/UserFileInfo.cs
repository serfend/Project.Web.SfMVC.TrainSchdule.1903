using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.FileEngine
{
	public class UserFileInfo : BaseEntity
	{
		/// <summary>
		/// 文件名
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 文件大小
		/// </summary>
		public long Length { get; set; }

		public DateTime Create { get; set; }

		/// <summary>
		/// 路径
		/// </summary>
		public string Path { get; set; }
	}
}