using DAL.Entities.Game_r3;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.GameR3
{
	public interface IGameR3Services
	{
		/// <summary>
		/// 获取用户的所有领取记录
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		Task<IEnumerable<GainGiftCode>> GetUserGiftCodeHistory(User user);
		/// <summary>
		/// 对所有的用户进行尝试领取操作
		/// </summary>
		/// <returns></returns>
		Task HandleAllUsersGiftCode();
		/// <summary>
		/// /player/simpleInfo?uid=
		/// 获取用户基本信息：{"msg":"success","code":0,"data":{"name":"\u6d1b\u5929\u5624","title":"\u5f71\u5fcd"}}
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		Task<UserInfo> UpdateUser(User user);
		/// <summary>
		/// 获取所有可用的礼品码
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<GiftCode>> GetAllValidGiftCodes();

		/// <summary>
		/// /player/giftCode?uid=&code=
		/// 执行领取操作：{"msg":"\u793c\u5305\u4e0d\u5b58\u5728","code":417}
		/// code不为0时表示失败
		/// code为425表示重复领取，不使得领取码失效
		/// </summary>
		/// <param name="user"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		Task<GiftCode> HandleCode(User user, GiftCode code);
		/// <summary>
		/// 分享新的礼品码
		/// </summary>
		/// <param name="user"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		Task<GiftCode> ShareCode(User user, GiftCode code);
		/// <summary>
		/// 修改用户礼品码领取间隔
		/// </summary>
		/// <param name="user"></param>
		/// <param name="interval"></param>
		/// <returns></returns>
		Task<User> UpdateHandleInterval(User user, long interval);
	}
}
