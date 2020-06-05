using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Vacations.Statistics.StatisticsNewApply
{
	public interface IStatisticsApplyBase : IDateStatistics, IHasIntId
	{
		/// <summary>
		/// 单位代码
		/// </summary>
		string CompanyCode { get; set; }

		/// <summary>
		/// 休假目的地 前两位代码（表示省）
		/// </summary>
		byte To { get; set; }

		/// <summary>
		/// 休假出发地 前两位代码（表示省）
		/// </summary>
		byte From { get; set; }

		/// <summary>
		/// 用户类型
		/// </summary>
		string Type { get; set; }

		/// <summary>
		/// 此种类休假的总天数
		/// </summary>
		int Day { get; set; }

		/// <summary>
		/// 数量
		/// </summary>
		int Value { get; set; }
	}
}