using Abp.Extensions;
using BLL.Interfaces.Permission;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BLL.Services.Permission
{
	public class PermissionServices : IPermissionServices
	{
		public List<Tuple<string, PermissionDescription>> Dicts { get; set; }

		public PermissionServices()
		{
			Type cur = typeof(DAL.Entities.DictionaryAllPermission);
			Dicts = new List<Tuple<string, PermissionDescription>>();
			// TODO 权限判断
			ExtractChildren(Dicts, cur, null);
		}

		private static void ExtractChildren(List<Tuple<string, PermissionDescription>> list, Type target, string currentKey, bool extractCurrent = false)
		{
			var children = target.GetNestedTypes();

			var des = target.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
			if (!currentKey.IsNullOrEmpty()) currentKey = $"{currentKey}/";
			if (extractCurrent) currentKey = $"{currentKey}{target.Name}:{des?.Description}";
			foreach (var permit in target.GetFields())
			{
				var item = permit.GetValue(permit.GetValue(null)) as PermissionDescription; // 静态字段直接加载
				list.Add(new Tuple<string, PermissionDescription>(currentKey, item));
			}
			if (children.Length == 0)
				return;
			foreach (var c in children)
				ExtractChildren(list, c, currentKey, true);
		}

		public List<Tuple<string, PermissionDescription>> AllPermission() => Dicts;
	}
}