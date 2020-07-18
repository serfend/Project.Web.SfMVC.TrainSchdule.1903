using DAL.Entities.Common;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Vacations.Statistics
{
	/// <summary>
	/// 统计结果存储
	/// </summary>
	public class StatisticsResultRecord : BaseEntityGuid, ICreateClientInfo
	{
		/// <summary>
		/// 统计结果，TODO 可考虑存储为文件
		/// </summary>
		public string Result { get; set; }

		/// <summary>
		/// 统计创建时间
		/// </summary>
		public DateTime Create { get; set; }

		/// <summary>
		/// 统计完成时间
		/// </summary>
		public DateTime Complete { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		public virtual User CreateBy { get; set; }

		/// <summary>
		/// 自动生成的描述，也可手动修改
		/// </summary>
		public string Description { get; set; }

		public string Ip { get; set; }
		public string Device { get; set; }
		public string UA { get; set; }
	}
}