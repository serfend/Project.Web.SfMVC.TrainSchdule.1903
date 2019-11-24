using System.Collections.Generic;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.UserInfo;

namespace BLL.Interfaces
{
	public interface IUserServiceDetail
	{
		/// <summary>
		/// 用户为主管的单位
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		IEnumerable<Company> InMyManage(string id);
		/// <summary>
		/// 获取用户的休假概况
		/// </summary>
		/// <param name="targetUser"></param>
		/// <returns></returns>
		UserVocationInfoVDto VocationInfo(User targetUser);
	}
}
