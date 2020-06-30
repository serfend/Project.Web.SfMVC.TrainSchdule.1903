using DAL.Entities.UserInfo;
using System;

namespace DAL.Entities
{
	/// <summary>
	/// xls渲染模板
	/// TODO 日后用户自定义模板上传
	/// </summary>
	public class XlsTemplete : BaseEntityGuid
	{
		/// <summary>
		/// 模板名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 文件名
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime Create { get; set; }

		/// <summary>
		/// 创建者
		/// </summary>
		public virtual User CreateBy { get; set; }
	}
}