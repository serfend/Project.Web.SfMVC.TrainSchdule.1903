using System.Collections.Generic;
using DAL.DTO.Apply;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Apply
{
	/// <summary>
	/// 
	/// </summary>
	public class ApplyListViewModel:ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public ApplyListDataModel Data { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class ApplyListDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<ApplySummaryDto> List { get; set; }
		/// <summary>
		/// 查询的总量
		/// </summary>
		public int TotalCount { get; set; }
	}
}
