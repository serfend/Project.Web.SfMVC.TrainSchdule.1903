using BLL.Helpers;
using DAL.DTO.Apply;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using TrainSchdule.ViewModels.System;
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
	public class QueryApplyByIdViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<Guid> Id { get; set; }
    }

}