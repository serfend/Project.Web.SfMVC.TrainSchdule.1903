using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.UserInfo.Resume
{
	public class UserResumeInfo : BaseEntityGuid
	{
		/// <summary>
		/// 级别更新简历
		/// </summary>
		public virtual IEnumerable<UserTitleResume> TitleResumes { get; set; }
	}
}