using System;
using System.ComponentModel.DataAnnotations;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Statistics
{
	/// <summary>
	///
	/// </summary>
	public class DeleteStatisticsViewModel : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		public DeleteStatisticsDataModel Data { get; set; }
	}

	/// <summary>
	/// 创建新的查询
	/// </summary>
	public class NewStatisticsViewModel : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		public NewStatisticsDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class NewStatisticsDataModel : DeleteStatisticsDataModel
	{
		/// <summary>
		///
		/// </summary>
		public string Description { get; set; }

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

	/// <summary>
	///
	/// </summary>
	public class DeleteStatisticsDataModel
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
	}
}