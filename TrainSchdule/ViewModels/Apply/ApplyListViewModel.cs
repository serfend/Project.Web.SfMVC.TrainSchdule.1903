using BLL.Helpers;
using DAL.DTO.Apply;
using DAL.QueryModel;
using System.Collections.Generic;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Apply
{
	/// <summary>
	///
	/// </summary>
	public class QueryApplyViewModel : QueryApplyDataModel
	{
		/// <summary>
		///
		/// </summary>
		public GoogleAuthDataModel Auth { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class ApplyListViewModel : ApiResult
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