using System.Collections.Generic;
using DAL.Entities;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 
	/// </summary>
	public class QueryPermissionsOutViewModel:ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public IDictionary<string, PermissionRegion> Data { get; set; }
	}
}
