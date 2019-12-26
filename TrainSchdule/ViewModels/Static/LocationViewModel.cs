using BLL.Helpers;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Static
{
	/// <summary>
	/// 
	/// </summary>
	public class LocationViewModel:ApiResult
	{
		/// <summary>
		/// 
		/// </summary>
		public LocationDataModel Data { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class LocationDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string ShortName { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Code { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int ParentCode { get; set; }
	}

}
