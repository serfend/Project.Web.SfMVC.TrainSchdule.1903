using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 
	/// </summary>
	public class UserIdByCidViewModel:ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public UserIdByCidDataModel Data { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class UserIdByCidDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public string Id { get; set; }
	}
}
