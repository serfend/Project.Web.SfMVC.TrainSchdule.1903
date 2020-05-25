using BLL.Helpers;
using System.Collections.Generic;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Company
{
	/// <summary>
	///
	/// </summary>
	public class AllChildViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public EntitiesListDataModel<CompanyChildDataModel> Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class CompanyChildDataModel
	{
		/// <summary>
		///
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///
		/// </summary>
		public string Code { get; set; }
	}
}