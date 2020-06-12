using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.Vacations.Statistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services.VacationStatistics
{
	public class StatisticsAppliesInfo : IStatisticsBase
	{
		/// <summary>
		/// 考虑到db需要，不使用构造函数
		/// </summary>
		public StatisticsAppliesInfo()
		{
		}

		public string Type { get; set; }
		public string CompanyCode { get; set; }
		public int Id { get; set; }
		public DateTime Target { get; set; }

		/// <summary>
		/// 休假来源人
		/// </summary>
		public User From { get; set; }

		/// <summary>
		/// 已休假天数
		/// </summary>
		public int Days { get; set; }

		/// <summary>
		/// 因召回而减少的假期
		/// </summary>
		public int RecallReduceDay { get; set; }
	}
}