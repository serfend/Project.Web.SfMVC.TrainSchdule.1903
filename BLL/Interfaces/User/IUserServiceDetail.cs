using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces
{
	public interface IUserServiceDetail
	{
		/// <summary>
		/// 用户为主管的单位
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		Task<Tuple<IEnumerable<Company>, int>> InMyManage(User user);

		/// <summary>
		/// 获取用户的休假概况
		/// </summary>
		/// <param name="targetUser"></param>
		/// <returns></returns>
		UserVacationInfoVDto VacationInfo(User targetUser);

		/// <summary>
		/// 更新头像
		/// </summary>
		/// <param name="targetUser"></param>
		/// <param name="newAvatar"></param>
		Task<Avatar> UpdateAvatar(User targetUser, string newAvatar);

		/// <summary>
		/// 按时间查询用户头像
		/// </summary>
		/// <param name="targetUser"></param>
		/// <param name="start"></param>
		/// <returns></returns>
		IEnumerable<Avatar> QueryAvatar(string targetUser, DateTime start);

		/// <summary>
		/// 检查是否有权限操作用户
		/// </summary>
		/// <param name="authUser"></param>
		/// <param name="modefyUser"></param>
		/// <param name="requireAuthRank"></param>
		/// <returns>当无权限时返回-1，否则返回当前授权用户可操作单位与目标用户的级别差</returns>
		int CheckAuthorizedToUser(User authUser, User modefyUser);

		/// <summary>
		/// 获取指定用户的家庭情况变更记录并进行修改
		/// </summary>
		/// <param name="user"></param>
		/// <param name="Callback"></param>
		/// <returns></returns>
		IEnumerable<AppUsersSettleModifyRecord> ModefyUserSettleModifyRecord(User user, Action<IEnumerable<AppUsersSettleModifyRecord>> Callback = null);

		/// <summary>
		/// 获取指定家庭变更记录并进行修改
		/// </summary>
		/// <param name="code"></param>
		/// <param name="Callback"></param>
		/// <param name="isDelete"></param>
		/// <returns></returns>
		AppUsersSettleModifyRecord ModefySettleModeyRecord(int code, Action<AppUsersSettleModifyRecord> Callback = null, bool isDelete = false);
	}
}