using BLL.Helpers;
using System.Collections.Generic;

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
		public AllChildDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class AllChildDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<CompanyChildDataModel> List { get; set; }
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