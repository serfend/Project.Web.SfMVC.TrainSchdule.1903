using Abp.Extensions;
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

		public List<Tuple<string, DAL.Entities.Permisstions.Permission>> AllPermissions { get; set; }
		public Dictionary<string, DAL.Entities.Permisstions.Permission> DictPermissions{get;set;}
		public PermissionServices(ApplicationDbContext context,IUserServiceDetail userServiceDetail)
		{
			Type cur = typeof(DAL.Entities.Permisstions.ApplicationPermissions);
			AllPermissions = new List<Tuple<string, DAL.Entities.Permisstions.Permission>>();
			DictPermissions = new Dictionary<string, DAL.Entities.Permisstions.Permission>();
			// TODO 权限判断
			ExtractChildren(cur, string.Empty, string.Empty);
            this.context = context;
            this.userServiceDetail = userServiceDetail;
        }

		private void ExtractChildren(Type target, string  currentKey,string currentDes, bool extractCurrent = false)
		{
			var children = target.GetNestedTypes();

			var des = target.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
			if (currentKey.IsNullOrEmpty()) currentKey = "permission";
			if (currentDes.IsNullOrEmpty()) currentDes = string.Empty;
			if (extractCurrent)
            {
				currentKey = $"{currentKey}.{target.Name}";
				currentDes = $"{currentDes}.{des?.Description}";
			}

			// 获取子类并加载，赋值key
			foreach (var permit in target.GetRuntimeFields())
			{
				var item = new DAL.Entities.Permisstions.Permission() { Key = currentKey, Description = currentDes };
				permit.SetValue(item, item);
				AllPermissions.Add(new Tuple<string, DAL.Entities.Permisstions.Permission>(currentKey, item));
				DictPermissions.Add(currentKey, item);
			}
			if (children.Length == 0)
				return;
			foreach (var c in children)
				ExtractChildren( c, currentKey, currentDes, true);
		}


        public IPermissionDescription CheckPermissions(User user, string permission, PermissionType permissionType, string companyCode)
        {
			var permissionNodes = permission.Split('.').ToList();
			var userPermission = context.PermissionsUsers.Where(p => p.UserId == user.Id);
			while (permissionNodes.Count > 0)
			{
				var current = string.Join('.', permissionNodes);
				var permit = userPermission.Where(i => i.Name == current).Where(i=>companyCode.StartsWith(i.Region)).Where(i=> i.Type.HasFlag(permissionType));
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