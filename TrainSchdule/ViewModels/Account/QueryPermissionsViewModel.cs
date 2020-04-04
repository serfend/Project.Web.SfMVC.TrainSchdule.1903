using BLL.Helpers;
using DAL.Entities;
using System.Collections.Generic;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	///
	/// </summary>
	public class QueryPermissionsOutViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public IDictionary<string, PermissionRegion> Data { get; set; }
	}
}