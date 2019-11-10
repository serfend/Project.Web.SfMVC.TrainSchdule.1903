using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Crontab
{
	/// <summary>
	/// 对一段时间的休假情况进行统计并存入到数据库
	/// </summary>
	public class BaseOnTimeVocationStatistics:ICrontabJob
	{
		private readonly ApplicationDbContext _context;

		public BaseOnTimeVocationStatistics(ApplicationDbContext context, DateTime start, DateTime end, string statisticsId=null)
		{
			_context = context;
			Start = start;
			End = end;
			StatisticsId = statisticsId??$"{Start.ToString("yyyyMMdd")}_{End.ToString("yyyyMMdd")}";
		}
		/// <summary>
		/// 生成统计ID
		/// </summary>
		public string StatisticsId { get;  }
		private DateTime Start { get; set; }
		private DateTime End { get; set; }

		public void Run()
		{
			//TODO 统计指定时间范围的休假情况
		}
	}
}
