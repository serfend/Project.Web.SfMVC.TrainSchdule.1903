using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Vacations.Statistics
{
	public interface IStatisticsBase : IHasIntId, IDateStatistics
	{
		/// <summary>
		/// 用户类型
		/// </summary>
		string Type { get; set; }

		/// <summary>
		/// 单位代码
		/// </summary>
		string CompanyCode { get; set; }
	}
}