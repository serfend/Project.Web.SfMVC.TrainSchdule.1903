using DAL.Entities.UserInfo.Settle;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.UserInfo.Resume
{
	public class UserSocialResume : BaseUserResume<Moment>
	{
		/// <summary>
		/// 简历作用对象
		/// </summary>
		public SocialResumeType SocialResumeType { get; set; }
	}

	/// <summary>
	/// 作用对象类型
	/// </summary>
	public enum SocialResumeType
	{
		Unknown,
		Self,
		Parent,
		Lover,
		LoversParent
	}
}