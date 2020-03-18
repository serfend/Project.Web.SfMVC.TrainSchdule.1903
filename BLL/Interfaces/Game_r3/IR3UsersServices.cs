using DAL.Entities.Game_r3;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.GameR3
{
	public interface IR3UsersServices
	{
		UserInfo UpdateUserInfo(UserInfo u);

		/// <summary>
		/// 通过userid获取用户信息
		/// </summary>
		/// <param name="userid"></param>
		/// <returns></returns>
		UserInfo GetUser(string userid);

		/// <summary>
		/// 返回以注册时间后先排序的会员
		/// </summary>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		Task<IEnumerable<UserInfo>> Members(int pageIndex, int pageSize);

		/// <summary>
		/// 返回以领取时间后先排序的记录
		/// </summary>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		Task<IEnumerable<GainGiftCode>> GainGiftCodeHistory(string userid, string code, int pageIndex, int pageSize);
	}
}