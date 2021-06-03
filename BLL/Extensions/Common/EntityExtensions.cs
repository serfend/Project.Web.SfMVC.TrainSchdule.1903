using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.ClientDevice;
using DAL.Entities.Permisstions;
using DAL.Entities.UserInfo;
using DAL.Entities.ZX.Grade;
using DAL.Entities.ZX.Phy;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions.Common
{
	/// <summary>
	///
	/// </summary>
	public static class EntityExtensions
	{
		/// <summary>
		/// 资源不存在
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static ApiResult NotExist(this IDbEntity model) => ActionStatusMessage.StaticMessage.ResourceNotExist;

		public static ApiResult NotExist(this Client model) => ActionStatusMessage.ClientDevice.Client.NotExist;
		public static ApiResult Exist(this Client model) => ActionStatusMessage.ClientDevice.Client.Exist;
		public static ApiResult NotExist(this Permission model) => ActionStatusMessage.PermissionMessage.Permission.NotExist;
		public static ApiResult Exist(this Permission model) => ActionStatusMessage.PermissionMessage.Permission.Exist;
		public static ApiResult NotExist(this PermissionRoleRelatePermission model) => ActionStatusMessage.PermissionMessage.RoleRelatePermission.NotExist;
		public static ApiResult Exist(this PermissionRoleRelatePermission model) => ActionStatusMessage.PermissionMessage.RoleRelatePermission.Exist;
		
		public static ApiResult NotExist(this PermissionsRoleRelate model) => ActionStatusMessage.PermissionMessage.RoleRelateRole.NotExist;
		public static ApiResult Exist(this PermissionsRoleRelate model) => ActionStatusMessage.PermissionMessage.RoleRelateRole.Exist;
		public static ApiResult NotExist(this PermissionsRole model) => ActionStatusMessage.PermissionMessage.Role.NotExist;
		public static ApiResult NotExist(this IPermissionDescription model) => ActionStatusMessage.PermissionMessage.Permission.NotExist;
		public static ApiResult NotExist(this PermissionsUserRelate model) => ActionStatusMessage.PermissionMessage.RoleRelateUser.NotExist;
		public static ApiResult Exist(this PermissionsUserRelate model) => ActionStatusMessage.PermissionMessage.RoleRelateUser.Exist;
		public static ApiResult NotExist(this Apply model) => ActionStatusMessage.ApplyMessage.NotExist;

		public static ApiResult NotExist(this RecallOrder model) => ActionStatusMessage.ApplyMessage.RecallMessage.NotExist;

		public static ApiResult NotExist(this User model) => ActionStatusMessage.UserMessage.NotExist;

		public static ApiResult NotLogin(this User model) => ActionStatusMessage.Account.Auth.Invalid.NotLogin;

		public static ApiResult NotExist(this GradePhyRecord model) => ActionStatusMessage.Grade.Record.NotExist;

		public static ApiResult NotExist(this GradeExam model) => ActionStatusMessage.Grade.Exam.NotExist;
	}
}