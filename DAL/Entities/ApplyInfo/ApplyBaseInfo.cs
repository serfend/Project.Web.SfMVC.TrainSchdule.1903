using DAL.Entities.UserInfo;
using System;

namespace DAL.Entities.ApplyInfo
{
	public class ApplyBaseInfo : BaseEntityGuid
	{
		public string RealName { get; set; }
		public string CompanyName { get; set; }
		public string DutiesName { get; set; }

		/// <summary>
		/// 申请创建的目标人
		/// </summary>
		public virtual DAL.Entities.UserInfo.User From { get; set; }

		/// <summary>
		/// 声请人单位情况
		/// </summary>
		public virtual Company Company { get; set; }

		/// <summary>
		/// 申请人职务情况
		/// </summary>
		public virtual Duties Duties { get; set; }

		/// <summary>
		/// 申请人家庭情况
		/// </summary>
		public virtual UserSocialInfo Social { get; set; }

		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 申请由何人创建
		/// </summary>
		public virtual User CreateBy { get; set; }

		public string FinnalAuditCompany { get; set; }
	}
}