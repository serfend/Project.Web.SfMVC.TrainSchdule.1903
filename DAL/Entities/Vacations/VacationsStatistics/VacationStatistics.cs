using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DAL.Entities.Vacations.VacationsStatistics
{
	public class VacationStatistics
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string Id { get; set; }

		public DateTime Start { get; set; }
		public DateTime End { get; set; }

		/// <summary>
		/// 统计期间使用的年度
		/// </summary>
		public int CurrentYear { get; set; }

		/// <summary>
		/// 通常自动生成，也可手动修改
		/// </summary>
		public string Description { get; set; }

		public virtual VacationStatisticsDescription RootCompanyStatistics { get; set; }
	}
}