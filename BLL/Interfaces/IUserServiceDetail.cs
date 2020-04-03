using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces
{
	public interface IUserServiceDetail
	{
		/// <summary>
		/// 用户为主管的单位
		/// </summary>
		/// <param name="user"></param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		IEnumerable<Company> InMyManage(User user, out int totalCount);

		/// <summary>
		/// 获取用户的休假概况
		/// </summary>
		/// <param name="targetUser"></param>
		/// <returns></returns>
		UserVocationInfoVDto VocationInfo(User targetUser);

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
	}
}