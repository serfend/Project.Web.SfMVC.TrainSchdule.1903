using System.Collections.Generic;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Company
{
	/// <summary>
	/// 
	/// </summary>
	public class AllChildViewModel:ApiDataModel
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
