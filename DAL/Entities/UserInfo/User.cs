using DAL.Entities.UserInfo.Resume;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.UserInfo
{
	public class User : UserID
	{
		/// <summary>
		/// 用户状态
		/// </summary>
		public AccountStatus AccountStatus { get; set; }

		/// <summary>
		/// 当状态为封禁时需要有此字段
		/// </summary>
		public DateTime StatusBeginDate { get; set; }

		/// <summary>
		/// 当状态为封禁时需要有此字段
		/// </summary>
		public DateTime StatusEndDate { get; set; }

		#region Properties

		[ForeignKey("ApplicationId")]
		public virtual UserApplicationInfo Application { get; set; }
		public Guid? ApplicationId { get; set; }

		[ForeignKey("BaseInfoId")]
		public virtual UserBaseInfo BaseInfo { get; set; }
        public Guid? BaseInfoId { get; set; }

        [ForeignKey("CompanyInfoId")]
		public virtual UserCompanyInfo CompanyInfo { get; set; }
        public Guid? CompanyInfoId { get; set; }

        [ForeignKey("SocialInfoId")]
		public virtual UserSocialInfo SocialInfo { get; set; }
        public Guid? SocialInfoId { get; set; }

        [ForeignKey("DiyInfoId")]
		public virtual UserDiyInfo DiyInfo { get; set; }
        public Guid? DiyInfoId { get; set; }

        [ForeignKey("ResumeInfoId")]
		public virtual UserResumeInfo ResumeInfo { get; set; }
        public Guid? ResumeInfoId { get; set; }
		/// <summary>
		/// 【冗余】用户按职务等排序值
		/// </summary>
		public long UserOrderRank { get; set; }

        #endregion Properties
    }

	public enum AccountStatus
	{
		Normal = 0,
		Banned = 1,
		Abolish = 2,
		DisableVacation = 4, // 当所选职务的 DisabledVacation 设置为True时，同步为所选职务的设置
		PrivateAccount = 8
	}
}