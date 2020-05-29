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
	public class SettleModefyRecordViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public SettleModefyRecordDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class SettleModefyRecordDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<AppUsersSettleModefyRecord> Records { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class SingleSettleModefyRecordViewModel
	{
		/// <summary>
		///
		/// </summary>
		public SingleSettleModefyRecordDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class SingleSettleModefyRecordDataModel
	{
		/// <summary>
		///
		/// </summary>
		public AppUsersSettleModefyRecord Record { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class ModefySingleSettleModefyRecordViewModel : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		public AppUsersSettleModefyRecord Record { get; set; }
	}
}