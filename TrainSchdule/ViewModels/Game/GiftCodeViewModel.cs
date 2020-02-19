using BLL.Helpers;
using DAL.Entities.Game_r3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Game
{

	public class GiftCodeViewModel : ApiResult
	{
		public GiftCodeDataModel Data { get; set; }
	}
	public class GiftCodeDataModel
	{
		public GiftCode Code { get; set; }
		public long GainStamp { get; set; }
		/// <summary>
		/// 用户昵称吧
		/// </summary>
		public string User { get; set; }
	}
	public class UserGiftCodeGainHistoryViewModel : ApiResult
	{
		public UserGiftCodeGainHistoryDataModel Data { get; set; }
	}
	public class UserGiftCodeGainHistoryDataModel
	{
		public IEnumerable<GiftCodeDataModel> List { get; set; }
	}
}
