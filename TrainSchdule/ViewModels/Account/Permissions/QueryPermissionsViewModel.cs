using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.Permisstions;
using System;
using System.Collections.Generic;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	/// 角色编辑
	/// </summary>
	public class PermissionsRoleViewModel : GoogleAuthViewModel
    {
		/// <summary>
		/// 角色
		/// </summary>
		public string Role { get; set; }
		/// <summary>
		/// 继承了哪些角色
		/// </summary>
		public IEnumerable<string> FromRoles { get; set; }
		/// <summary>
		/// 被哪些角色继承
		/// </summary>
		public IEnumerable<string> ToRoles { get; set; }
		/// <summary>
		/// 包含哪些权限
		/// </summary>
		public IEnumerable<PermissionBaseItem> Permissions { get; set; }

		/// <summary>
		/// 是否删除
		/// </summary>
		public bool IsRemove { get; set; }
		/// <summary>
		/// 创建人
		/// </summary>
		public string CreateBy { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime Create { get; set; }
	}
	/// <summary>
	/// 为角色添加权限
	/// </summary>
	public class RoleAttachPermissionViewModel: GoogleAuthViewModel
	{
		/// <summary>
		/// 角色名称
		/// </summary>
		public string Role { get; set; }
		/// <summary>
		/// 关联角色
		/// </summary>
		public string RelateRole { get; set; }
		/// <summary>
		/// 关联权限
		/// </summary>
		public PermissionBaseItem Permission { get; set; }
		/// <summary>
		/// 移除
		/// </summary>
		public bool IsRemove { get; set; }
	}
	/// <summary>
	///
	/// </summary>
	public class QueryPermissionsOutViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public QueryPermissionsOutDataModel Data { get; set; }
	}
	/// <summary>
	/// 鉴权
	/// </summary>
	public class PermissionCheckViewModel { 
		/// <summary>
		/// 目标用户
		/// </summary>
		public string User { get; set; }
		/// <summary>
		/// 权限名称
		/// </summary>
		public string Permission { get; set; }
		/// <summary>
		/// 作用域
		/// </summary>
		public string Region { get; set; }
		/// <summary>
		/// 操作
		/// </summary>
		public PermissionType PermissionType { get; set; }
	}
	/// <summary>
	/// 转model
	/// </summary>
	public static class PermissionsExtensions
    {
        /// <summary>
        /// 单个权限转换
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fromRoles"></param>
        /// <param name="ToRoles"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static PermissionsRoleViewModel ToModel(this PermissionsRole model,IEnumerable<string> fromRoles,IEnumerable<string>ToRoles,IEnumerable<PermissionBaseItem> permissions) => new PermissionsRoleViewModel() {
			CreateBy = model.CreateById,
			Role = model.Name,
			Create = model.Create,
			FromRoles = fromRoles,
			ToRoles = ToRoles,
			Permissions = permissions
		};
		/// <summary>
		/// 用户单个权限转换
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static PermissionsDataModel ToModel(this PermissionsUser model) => new PermissionsDataModel() { 
			Permission=model.Name,
			Region=model.Region,
			Type=model.Type,
			UserId=model.UserId
		};
		/// <summary>
		/// 用户单个转换角色
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static RolesDataModel ToModel(this PermissionsUserRelate model) => new RolesDataModel() { 
			UserId=model.UserId,
			Create=model.Create,
			Role=model.RoleName
		};
	}
	/// <summary>
	/// 用户权限和角色
	/// </summary>
	public class QueryPermissionsOutDataModel
    {
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<PermissionsDataModel> Permissions { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<RolesDataModel> Roles { get; set; }
    }
	/// <summary>
	/// 单个权限
	/// </summary>
	public class PermissionsDataModel {
		/// <summary>
		/// 用户id
		/// </summary>
		public string UserId { get; set; }
		/// <summary>
		/// 权限名称
		/// </summary>
		public string Permission { get; set; }
		/// <summary>
		/// 单位作用域
		/// </summary>
		public string Region { get; set; }
		/// <summary>
		/// 权限操作类型
		/// </summary>
		public PermissionType Type { get; set; }
	}
	/// <summary>
	/// 单个角色
	/// </summary>
	public class RolesDataModel
    {
		/// <summary>
		/// 用户id
		/// </summary>
		public string UserId { get; set; }
		/// <summary>
		/// 角色名称
		/// </summary>
		public string Role { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime Create { get; set; }
    }
}