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
		/// <param name="id"></param>
		/// <returns></returns>
		IEnumerable<Company> InMyManage(string id,out int totalCount);
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
		/// 通过用户获取其最新的头像（TODO后期可新增根据头像时间获取头像组）
		/// </summary>
		/// <param name="targetUser"></param>
		/// <returns></returns>
		Task<Avatar> GetAvatar(User targetUser);
	}
}
