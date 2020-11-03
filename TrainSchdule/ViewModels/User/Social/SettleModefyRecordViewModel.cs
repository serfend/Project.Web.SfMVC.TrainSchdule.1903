using BLL.Helpers;
using DAL.Entities.UserInfo.Settle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.User.Social
{
	/// <summary>
	///
	/// </summary>
	public class SettleModifyRecordViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public SettleModifyRecordDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class SettleModifyRecordDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<AppUsersSettleModifyRecord> Records { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class SingleSettleModifyRecordViewModel
	{
		/// <summary>
		///
		/// </summary>
		public SingleSettleModifyRecordDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class SingleSettleModifyRecordDataModel
	{
		/// <summary>
		///
		/// </summary>
		public AppUsersSettleModifyRecord Record { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class ModifySingleSettleModifyRecordViewModel : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		public AppUsersSettleModifyRecord Record { get; set; }
	}
}