using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.Permisstions;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	/// <summary>
	/// 用户操作记录,需要引用private readonly IHttpContextAccessor _httpContextAccessor;
	/// </summary>
	public interface IUserActionServices
	{
		/// <summary>
		/// 登记日志并返回失败信息
		/// </summary>
		/// <param name="action"></param>
		/// <param name="message"></param>
		/// <param name="_userActionServices"></param>
		/// <returns></returns>
		ApiResult LogNewActionInfo(UserAction action, ApiResult message);

		/// <summary>
		/// 创建一个记录
		/// </summary>
		/// <param name="operation"></param>
		/// <param name="user"></param>
		/// <param name="success"></param>
		/// <returns></returns>
		UserAction Log(UserOperation operation, string username, string Description, bool success = false, ActionRank rank = ActionRank.Debug);
		Task<UserAction>LogAsync(UserOperation operation, string username, string Description, bool success = false, ActionRank rank = ActionRank.Debug);

		/// <summary>
		/// 当创建的记录状态为不成功时，需要将记录状态置为成功
		/// </summary>
		/// <param name="action"></param>
		/// <returns></returns>
		UserAction Status(UserAction action, bool success, string description = null);
		/// <summary>
		/// 授权记录
		/// </summary>
		/// <param name="authUser">授权方id</param>
		/// <param name="permission">授权到何类型</param>
		/// <param name="operation">何操作</param>
		/// <param name="targetUserCompanyCode">被授权方单位</param>
		/// <param name="description">描述</param>
		/// <returns></returns>
		bool Permission(User authUser, DAL.Entities.Permisstions.Permission permission, PermissionType operation, string targetUserCompanyCode, string description = null);
		Task<bool> PermissionAsync(User authUser, DAL.Entities.Permisstions.Permission permission, PermissionType operation, string targetUserCompanyCode, string description = null);
		Task<IEnumerable<UserAction>> Query(QueryUserActionViewModel model);
	}
}