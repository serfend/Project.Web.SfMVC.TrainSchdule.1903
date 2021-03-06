using DAL.Entities.UserInfo;
using System;
using System.ComponentModel.DataAnnotations.Schema;

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
		[ForeignKey("FromId")]
		public virtual DAL.Entities.UserInfo.User From { get; set; }
		public string FromId { get; set; }
		/// <summary>
		/// 申请人单位情况
		/// </summary>
		[ForeignKey("CompanyCode")]
		public virtual Company Company { get; set; }
		public string CompanyCode { get; set; }

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
		[ForeignKey("CreateById")]
		public virtual User CreateBy { get; set; }
		public string CreateById { get; set; }

		public string FinnalAuditCompany { get; set; }
	}
}