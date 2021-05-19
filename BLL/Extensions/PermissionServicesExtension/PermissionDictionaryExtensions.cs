using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions.PermissionServicesExtension
{
    public static class PermissionDictionaryExtensions
    {

		/// <summary>
		/// 当前程序集所有权限
		/// </summary>
		/// <returns></returns>
		public static List<Tuple<string, DAL.Entities.Permisstions.Permission>> AllPermissions { get; set; }
		/// <summary>
		/// 当前程序集所有权限字典
		/// </summary>
		public static Dictionary<string, DAL.Entities.Permisstions.Permission> DictPermissions { get; set; }
		static PermissionDictionaryExtensions()
        {
			Type cur = typeof(DAL.Entities.Permisstions.ApplicationPermissions);
			AllPermissions = new List<Tuple<string, DAL.Entities.Permisstions.Permission>>();
			DictPermissions = new Dictionary<string, DAL.Entities.Permisstions.Permission>();
			// TODO 权限判断
			ExtractChildren(cur, string.Empty, string.Empty);
		}

		private static void ExtractChildren(Type target, string currentKey, string currentDes, bool extractCurrent = false)
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
			foreach (
				var (permit, item) in from permit in target.GetRuntimeFields()
				let item = new DAL.Entities.Permisstions.Permission() { Key = currentKey, Description = currentDes }
				select (permit, item)
			)
            {
                permit.SetValue(item, item);
                AllPermissions.Add(new Tuple<string, DAL.Entities.Permisstions.Permission>(currentKey, item));
                DictPermissions.Add(currentKey, item);
            }

            if (children.Length == 0)
				return;
			foreach (var c in children)
				ExtractChildren(c, currentKey, currentDes, true);
		}

	}
}
