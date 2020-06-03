using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Vacations.Statistics
{
	public interface IDateStatistics
	{
		/// <summary>
		/// 统计目标 仅统计到天
		/// </summary>
		DateTime Target { get; set; }
	}
}