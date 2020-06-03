using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Vacations.VacationsStatistics
{
	public class VacationStatisticsDescriptionData : BaseEntity
	{
		/// <summary>
		/// 按职务类别分类统计<see cref="DAL.Entities.UserInfo.UserCompanyTitle.TitleType"/>
		/// </summary>
		public string TitleType { get; set; }

		/// <summary>
		/// 单位本级人员数量<see cref="单位人员总数"/>
		/// </summary>
		public int MembersCount { get; set; }

		/// <summary>
		/// 已完成全年休假指标的人数<see cref="休满假数（率）"/>
		/// </summary>
		public int CompleteYearlyVacationCount { get; set; }

		/// <summary>
		/// 应当休假总天数<see cref="应（实）休总天数"/>
		/// </summary>
		public int CompleteVacationExpectDayCount { get; set; }

		/// <summary>
		/// 实际休假总天数<see cref="应（实）休总天数"/>
		/// </summary>
		public int CompleteVacationRealDayCount { get; set; }

		/// <summary>
		/// 休假率低于60%人员数<see cref="休假率低于60%数"/>
		/// </summary>
		public int MembersVacationDayLessThanP60 { get; set; }

		/// <summary>
		/// 申请通过的数量
		/// </summary>
		public int ApplyCount { get; set; }

		/// <summary>
		/// 申请通过的人数
		/// </summary>
		public int ApplyMembersCount { get; set; }

		/// <summary>
		/// 申请通过的天数
		/// </summary>
		public int ApplySumDayCount { get; set; }
	}
}