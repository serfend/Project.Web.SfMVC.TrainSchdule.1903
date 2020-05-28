using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DAL.Entities.Vacations.VacationsStatistics
{
	public class VacationStatisticsDescription : BaseEntity
	{
		public string StatisticsId { get; set; }

		/// <summary>
		/// 单位下属单位休假情况
		/// </summary>
		[NotMapped]
		public virtual IEnumerable<VacationStatisticsDescription> Childs { get; set; }

		/// <summary>
		/// 单位
		/// </summary>
		public virtual Company Company { get; set; }

		/// <summary>
		/// 单位本级的休假记录
		/// </summary>
		[NotMapped]
		public virtual IQueryable<ApplyInfo.Apply> Applies { get; set; }

		/// <summary>
		/// 包含子单位数据
		/// </summary>
		public virtual IEnumerable<VacationStatisticsDescriptionData> Data { get; set; }
	}
}