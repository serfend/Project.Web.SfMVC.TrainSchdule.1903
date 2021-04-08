using DAL.Entities;
using DAL.Entities.Permisstions;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Interfaces.Permission
{
	public interface IPermissionServices
	{
		/// <summary>
		/// 当前程序集所有权限
		/// </summary>
		/// <returns></returns>
		List<Tuple<string, DAL.Entities.Permisstions.Permission>> AllPermissions { get; set; }
		/// <summary>
		/// 当前程序集所有权限字典
		/// </summary>
		Dictionary<string, DAL.Entities.Permisstions.Permission> DictPermissions { get; set; }
		/// <summary>
		/// 通过名称获取权限
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		DAL.Entities.Permisstions.Permission GetPermissionByName(string name);
		/// <summary>
		/// 角色创建，被用户创建
		/// </summary>
		/// <param name="role"></param>
		/// <param name="createBy"></param>
		/// <param name="isRemove"></param>
		/// <returns></returns>
		PermissionsRole RoleModify(string role, string createBy, bool isRemove);
		/// <summary>
		/// 为角色关联权限，被用户创建
		/// </summary>
		/// <param name="role">目标角色</param>
		/// <param name="permission">关联到权限</param>
		/// <returns></returns>
		PermissionRoleRelatePermission RoleRelatePermissions(string role, IPermissionDescription permission);
		/// <summary>
		/// 角色授权，继承其他角色
		/// </summary>
		/// <param name="from">授权方</param>
		/// <param name="to">被授权方</param>
		/// <param name="isRemove">是否是删除</param>
		/// <returns></returns>
		PermissionsRoleRelate RoleRelateRole(string from,string to,bool isRemove);
		/// <summary>
		/// 角色授权，通过某用户授权给另一用户
		/// </summary>
		/// <param name="from">授权何角色</param>
		/// <param name="userAuthBy">授权方用户</param>
		/// <param name="userAuthTo">被授权方用户</param>
		/// <returns></returns>
		PermissionsRoleRelate RoleRelateRole(string from, User userAuthBy,string userAuthTo);
		/// <summary>
		/// 同步角色应有权限
		/// 角色/权限的授驳时执行
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		void SyncRolePermissions(string role);
		/// <summary>
		/// 用户角色指定，被用户创建
		/// </summary>
		/// <param name="user">指定用户</param>
		/// <param name="role">授权到角色</param>
		/// <param name="isRemove">是否是删除</param>
		/// <returns></returns>
		PermissionsUserRelate UserRalteRole(string user, string role,bool isRemove);
		/// <summary>
		/// 同步用户应有权限
		/// 用户指定角色、角色发生变动时执行
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		void SyncUserPermissions(string user);
		/// <summary>
		/// 权限判断，逐级到根节点
		/// </summary>
		/// <param name="user">指定用户</param>
		/// <param name="permission">判断权限</param>
		/// <param name="permissionType">操作类型</param>
		/// <param name="companyCode">单位作用域</param>
		/// <returns></returns>
		IPermissionDescription CheckPermissions(User user, string permission,PermissionType permissionType, string companyCode);
		/// <summary>
		/// 获取用户所有的权限
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		IQueryable<PermissionsUser> GetPermissions(User user);
		/// <summary>
		/// 获取用户所有的角色
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		IQueryable<PermissionsUserRelate> GetRoles(User user);

		/// <summary>
		/// 获取权限最高层级单位
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		IEnumerable<string> RolePermissionCompany(string role); 
	}
}