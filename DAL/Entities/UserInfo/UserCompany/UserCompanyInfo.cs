using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.UserInfo
{
	public class UserCompanyInfo : BaseEntityGuid
	{
		/// <summary>
		/// 用户所处的单位
		/// </summary>
		[ForeignKey("CompanyCode")]
		public virtual Company Company { get; set; }
		public string CompanyCode { get; set; }
		/// <summary>
		/// 用户的编制单位
		/// </summary>
		public virtual Company CompanyOfManage { get; set; }
		public string CompanyOfManageCode { get; set; }
		//[Required(ErrorMessage = "未输入职务信息")]
		public virtual Duties Duties { get; set; }

		/// <summary>
		/// 职务等级
		/// </summary>
		//[Required(ErrorMessage = "未输入职务等级")]
		public virtual UserCompanyTitle Title { get; set; }

		/// <summary>
		/// 职务等级时间
		/// </summary>
		//[Required(ErrorMessage = "未输入职务等级时间")]
		public DateTime? TitleDate { get; set; }
	}
}