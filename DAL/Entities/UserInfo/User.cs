﻿using DAL.Entities.UserInfo.Resume;
using System;

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

		public virtual UserApplicationInfo Application { get; set; }
		public virtual UserBaseInfo BaseInfo { get; set; }
		public virtual UserCompanyInfo CompanyInfo { get; set; }
		public virtual UserSocialInfo SocialInfo { get; set; }
		public virtual UserDiyInfo DiyInfo { get; set; }
		public virtual UserResumeInfo ResumeInfo { get; set; }

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