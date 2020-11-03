using DAL.Entities.UserInfo;
using System;

namespace DAL.Entities.FileEngine
{
	public class UserFileInfo : BaseEntityGuid
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
		/// 创建人可查询本人创建的文件
		/// </summary>
		public virtual User CreateBy { get; set; }

		/// <summary>
		/// 父文件夹（文件=id=f.Parent.id&&path=f.path）
		/// </summary>
		public virtual UserFileInfo Parent { get; set; }

		/// <summary>
		/// 文件来源ip
		/// </summary>
		public string FromClient { get; set; }

		/// <summary>
		/// 当文件需要修改或者删除时需要验证身份
		/// </summary>
		public Guid ClientKey { get; set; }
	}
}