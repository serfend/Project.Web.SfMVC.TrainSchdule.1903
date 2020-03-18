using DAL.Entities.BBS;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces.BBS
{
	public interface ISignInServices
	{
		/// <summary>
		/// 签到
		/// </summary>
		void SignIn(string signId);

		/// <summary>
		/// 根据id和查询，若id为null或date为null则无限定查询（更多）
		/// </summary>
		/// <param name="signId"></param>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		IEnumerable<SignIn> QueryByDate(string signId, DateTime startDate, DateTime endDate);

		/// <summary>
		/// 检查日期范围内第一次签到
		/// </summary>
		/// <param name="signId"></param>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		SignIn QuerySingle(string signId, DateTime startDate, DateTime endDate);
	}
}