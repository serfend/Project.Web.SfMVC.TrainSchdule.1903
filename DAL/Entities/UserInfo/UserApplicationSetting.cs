using DAL.Entities.Common;
using System;

namespace DAL.Entities.UserInfo
{
	public class UserApplicationSetting : BaseEntityGuid
	{
		/// <summary>
		/// 用户两次提交申请的时间应间隔不少于一定时间
		///  TODO 使用集中分发管理
		/// </summary>
		public DateTime? LastSubmitApplyTime { get; set; }
	}
}