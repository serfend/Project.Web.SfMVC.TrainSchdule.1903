using Abp.Extensions;
using BLL.Extensions.PermissionServicesExtension;
using BLL.Interfaces;
using BLL.Interfaces.Permission;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Permisstions;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BLL.Services.Permission
{
	public partial class PermissionServices : IPermissionServices
	{
		private readonly ApplicationDbContext context;
		private readonly IUserServiceDetail userServiceDetail;
		

		public PermissionServices(ApplicationDbContext context,IUserServiceDetail userServiceDetail)
		{
            this.context = context;
            this.userServiceDetail = userServiceDetail;
			var t = PermissionDictionaryExtensions.DictPermissions.Count;
			Console.WriteLine($"permission loaded {t}");

		}


        public IPermissionDescription CheckPermissions(string userid, string permission, PermissionType permissionType, string companyCode)
        {
			var permissionNodes = permission.Split('.').ToList();
			var userPermission = context.PermissionsUsers.Where(p => p.UserId == userid);
			while (permissionNodes.Count > 0)
			{
				var current = string.Join('.', permissionNodes);
				var permit = userPermission.Where(i => i.Name == current).Where(i=> i.Type.HasFlag(permissionType));
				if (!companyCode.IsNullOrEmpty()) permit = permit.Where(i => companyCode.StartsWith(i.Region)); // 如果传入空单位，则只要有此权限即可通过
				var r = permit.FirstOrDefault();
				if (r!=null) return new PermissionBaseItem() { 
					Name = r.Name,
					Region = r.Region,
					Type = r.Type
				};
				permissionNodes.RemoveAt(permissionNodes.Count - 1);
			}
			return null;
		}

	}
}