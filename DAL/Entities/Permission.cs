﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DAL.Entities
{
	public class Permissions:BaseEntity
	{
		public string Regions { get; set; }
		/// <summary>
		/// 用户角色，当角色为System时拥有所有权限，当角色为Admin时拥有所有再说吧
		/// </summary>
		public string Role { get; set; }
	}

	public enum Operation
	{
		Create,
		Update,
		Remove,
		Query
	}

	public static class PermissionList
	{
		public static class User
		{
			public static PermissionDescription Application=new PermissionDescription("User.Application","密码、安全码等敏感信息");
		}
	}

	public class PermissionDescription
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public PermissionDescription(string name, string description)
		{
			this.Name = name;
			this.Description = description;
		}
	}
	public static class PermissionCheckExtension
	{
		/// <summary>
		/// 基于授权人<see cref="permissions"/>权限，对被授权人<see cref="grantBy"/>权限进行更新到序列值<see cref="newSerializeRaw"/>
		/// </summary>
		/// <param name="permissions"></param>
		/// <param name="newSerializeRaw"></param>
		/// <param name="grantBy"></param>
		/// <returns></returns>
		public static bool Update(this Permissions permissions, string newSerializeRaw, Permissions grantBy)
		{
			if (grantBy.Role != "Admin")
			{
				var dicList = GetRegionList(permissions);
				var grantList = GetRegionList(grantBy);

				var askForList = ToRegionList(newSerializeRaw);
				if (askForList.Any(region =>
				{
					var (key, value) = region;
					var keyItem = new PermissionDescription(key, "");
					var grantPermission = GetRegion(grantList, keyItem);
					var nowPermission = GetRegion(dicList, keyItem);
					if (!CheckIfHavePermission(grantPermission.Update.Union(nowPermission.Update), value.Update)) return true;
					if (!CheckIfHavePermission(grantPermission.Create.Union(nowPermission.Create), value.Create)) return true;
					if (!CheckIfHavePermission(grantPermission.Query.Union(nowPermission.Query), value.Query)) return true;
					if (!CheckIfHavePermission(grantPermission.Remove.Union(nowPermission.Remove), value.Remove)) return true;
					return false;
				})) return false;
			}
			
			Update(permissions,newSerializeRaw);
			return true;
		}

		private static bool CheckIfHavePermission(IEnumerable<string> granter, IEnumerable<string> expectTo)
		{
			return expectTo.All(p => granter.Any(p.StartsWith));
		}
		private static void Update(this Permissions permissions, string newSerializeRaw) =>
			permissions.Regions = newSerializeRaw;

		/// <summary>
		/// 获取当前用户的权限域
		/// </summary>
		/// <param name="permissions"></param>
		/// <returns></returns>
		private static IDictionary<string, PermissionRegion> GetRegionList(Permissions permissions)
		{
			var raw = permissions.Regions;
			if (raw==null)return new Dictionary<string, PermissionRegion>();
			return ToRegionList(raw);
		}

		public static IDictionary<string, PermissionRegion> ToRegionList(string raw)
		{
			return JsonConvert.DeserializeObject<IDictionary<string, PermissionRegion>>(raw) ?? new Dictionary<string, PermissionRegion>();
		}
		/// <summary>
		/// 获取指定权限列表
		/// </summary>
		/// <param name="permissions"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		private static PermissionRegion GetRegion(Permissions permissions, PermissionDescription key)
		{
			var dicList = GetRegionList(permissions);
			return GetRegion(dicList, key);
		}

		private static PermissionRegion GetRegion(IDictionary<string, PermissionRegion> dicList, PermissionDescription key)
		{
			if (!dicList.ContainsKey(key.Name)) return null;
			var dic = dicList[key.Name];

			return dic;
		}
		/// <summary>
		/// 新增权限
		/// </summary>
		/// <param name="permissions"></param>
		/// <param name="key"></param>
		/// <param name="operation"></param>
		/// <param name="targetUserCompanyCode"></param>
		public static void AddNew(this Permissions permissions, PermissionDescription key, Operation operation,
			string targetUserCompanyCode)
		{
			if (Check(permissions, key, operation, targetUserCompanyCode)) return;
			var dicList = GetRegionList(permissions);
			if(!dicList.ContainsKey(key.Name)) dicList[key.Name]=new PermissionRegion()
			{
				Create = new List<string>(),
				Query = new List<string>(),
				Remove = new List<string>(),
				Update = new List<string>()
			};
			var dic = dicList[key.Name];
			switch (operation)
			{
				case Operation.Create: dic.Create=dic.Create.Append(targetUserCompanyCode); break;
				case Operation.Query: dic.Query=dic.Query.Append(targetUserCompanyCode); break;
				case Operation.Remove: dic.Remove=dic.Remove.Append(targetUserCompanyCode); break;
				case Operation.Update: dic.Update=dic.Update.Append(targetUserCompanyCode); break;
			}
			Update(permissions,JsonConvert.SerializeObject(dicList));
		}

		public static void Remove(this Permissions permissions, PermissionDescription key, Operation operation,
			string targetUserCompanyCode)
		{
			if (!Check(permissions, key, operation, targetUserCompanyCode)) return;
			var dicList = GetRegionList(permissions);
			if (!dicList.ContainsKey(key.Name)) dicList[key.Name] = new PermissionRegion()
			{
				Create = new List<string>(),
				Query = new List<string>(),
				Remove = new List<string>(),
				Update = new List<string>()
			};
			
			var dic = dicList[key.Name];
			switch (operation)
			{
				case Operation.Create:dic.Create=dic.Create.Where(t=>!t.StartsWith(targetUserCompanyCode)); break;
				case Operation.Query: dic.Query= dic.Query.Where(t => !t.StartsWith(targetUserCompanyCode)); break;
				case Operation.Remove: dic.Remove=dic.Remove.Where(t => !t.StartsWith(targetUserCompanyCode)); break;
				case Operation.Update: dic.Update=dic.Update.Where(t => !t.StartsWith(targetUserCompanyCode)); break;
			}
			Update(permissions, JsonConvert.SerializeObject(dicList));
		}
		public static bool Check(this Permissions permissions, PermissionDescription key,Operation operation,string targetUserCompanyCode)
		{
			var dic = GetRegion(permissions, key);
			if (dic == null) return false;
			IEnumerable<string> list;
			switch (operation)
			{
				case Operation.Create: list = dic.Create;break;
				case Operation.Query: list = dic.Query; break;
				case Operation.Remove: list = dic.Remove; break;
				case Operation.Update: list = dic.Update; break;
				default: return false;
			}

			return list.Any(targetUserCompanyCode.StartsWith);
		}

		
		public static bool Check(this Permissions permissions, PermissionDescription key, Operation operation,
			Entities.Company targetUserCompany) => Check(permissions, key, operation, targetUserCompany?.Code);
		public static bool Check(this Permissions permissions, PermissionDescription key, Operation operation,
			Entities.UserInfo.User user) => Check(permissions, key, operation, user.CompanyInfo.Company?.Code);
	}


	public class PermissionRegion
	{
		public IEnumerable<string> Create { get; set; }
		public IEnumerable<string> Update { get; set; }
		public IEnumerable<string> Remove { get; set; }
		public IEnumerable<string> Query { get; set; }
	}

}