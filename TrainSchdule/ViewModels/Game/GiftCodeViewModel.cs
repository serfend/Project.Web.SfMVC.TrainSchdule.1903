using BLL.Helpers;
using DAL.Entities.Game_r3;
using System.Collections.Generic;

namespace TrainSchdule.ViewModels.Game
{
	/// <summary>
	///
	/// </summary>
	public class GiftCodeViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public GiftCodeDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class GiftCodeDataModel
	{
		/// <summary>
		///
		/// </summary>
		public GiftCode Code { get; set; }

		/// <summary>
		///
		/// </summary>
		public long GainStamp { get; set; }

		/// <summary>
		/// 用户昵称吧
		/// </summary>
		public string User { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserGiftCodeGainHistoryViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserGiftCodeGainHistoryDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserGiftCodeGainHistoryDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<GiftCodeDataModel> List { get; set; }
	}
}