using BLL.Helpers;
using System.Collections.Generic;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Static
{
	/// <summary>
	/// 
	/// </summary>
	public class LocationChildrenViewModel:ApiResult
	{
		/// <summary>
		/// 
		/// </summary>
		public LocationChildrenDataModel Data { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class LocationChildrenDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<LocationChildNodeDataModel> List { get; set; }
		/// <summary>
		/// 总量
		/// </summary>
		public int TotalCount { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class LocationChildNodeDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Code { get; set; }
	}
}
