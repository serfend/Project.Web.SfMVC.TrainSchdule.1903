using System.Collections.Generic;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Static
{
	/// <summary>
	/// 
	/// </summary>
	public class LocationChildrenViewModel:ApiDataModel
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
