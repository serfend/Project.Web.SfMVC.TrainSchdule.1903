using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Statistics
{
	/// <summary>
	/// 创建新的查询
	/// </summary>
	public class NewStatisticsViewModel:GoogleAuthViewModel
	{
		public NewStatisticsDataModel Data { get; set; }
	}
	public class NewStatisticsDataModel
	{
		/// <summary>
		/// 统计的单位，默认为根节点单位
		/// </summary>
		public string CompanyCode { get; set; }
		/// <summary>
		/// 统计的id，用于后续查询使用
		/// </summary>
		[Required]
		public string StatisticsId { get; set; }
		/// <summary>
		/// 统计开始时间
		/// </summary>
		[Required]

		public DateTime Start { get; set; }
		/// <summary>
		/// 统计结束时间
		/// </summary>
		[Required]

		public DateTime End { get; set; }
	}
}
