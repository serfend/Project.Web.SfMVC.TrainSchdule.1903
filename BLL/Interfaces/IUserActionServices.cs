using DAL.Entities;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Interfaces
{
	/// <summary>
	/// 用户操作记录,需要引用private readonly IHttpContextAccessor _httpContextAccessor;
	/// </summary>
	public interface IUserActionServices
	{
		/// <summary>
		/// 创建一个记录
		/// </summary>
		/// <param name="operation"></param>
		/// <param name="user"></param>
		/// <param name="success"></param>
		/// <returns></returns>
		UserAction Log(UserOperation operation, string username,string Description, bool success = false);
		/// <summary>
		/// 当创建的记录状态为不成功时，需要将记录状态置为成功
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		UserAction Status(UserAction action, bool success, string description = null);
		/// <summary>
		/// 授权记录
		/// </summary>
		/// <param name="permissions"></param>
		/// <param name="key"></param>
		/// <param name="operation"></param>
		/// <param name="permissionUserName"></param>
		/// <param name="targetUserCompanyCode"></param>
		/// <returns></returns>
		bool Permission(Permissions permissions, PermissionDescription key, Operation operation, string permissionUserName,string targetUserCompanyCode);
	}
}
