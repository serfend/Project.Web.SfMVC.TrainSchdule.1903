using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace DAL.Entities
{
	public class Permissions : BaseEntityGuid
	{
		//TODO 之后可能加上角色控制
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

	public static class DictionaryAllPermission
	{
		public static class User
		{
			public static PermissionDescription Application = new PermissionDescription("User.Application", "用户的密码、安全码等敏感信息");
			public static PermissionDescription BaseInfo = new PermissionDescription("User.BaseInfo", "用户的基本信息");
			public static PermissionDescription SocialInfo = new PermissionDescription("User.SocialInfo", "用户的家庭情况信息");
		}

		public static class Apply
		{
			public static PermissionDescription Default = new PermissionDescription("Apply.Default", "休假申请的编辑权限");
			public static PermissionDescription AuditStream = new PermissionDescription("Apply.AuditStream", "申请审批流的编辑权限");
		}

		public static class Grade
		{
			public static PermissionDescription Subject = new PermissionDescription("Grade.Subject", "科目标准的编辑权限");
			public static PermissionDescription Exam = new PermissionDescription("Grade.Exam", "考核的编辑权限");
			public static PermissionDescription Record = new PermissionDescription("Grade.Record", "成绩记录的编辑权限");
		}

		public static class Resources
		{
			public static PermissionDescription Default = new PermissionDescription("Resources.Default", "资源的编辑权限");
			public static PermissionDescription ShortUrl = new PermissionDescription("Resources.ShortUrl", "短网址的编辑权限");
		}
	}

	public class PermissionDescription
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public PermissionDescription(string name, string description)
		{
			Name = name;
			Description = description;
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
				var dicList = permissions.GetRegionList();
				var grantList = grantBy.GetRegionList();

				var askForList = ToRegionList(newSerializeRaw);
				if (askForList.Any(region =>
				{
					var (key, value) = region;
					var keyItem = new PermissionDescription(key, "");
					var grantPermission = GetRegion(grantList, keyItem);
					var nowPermission = GetRegion(dicList, keyItem);
					if (!CheckIfHavePermission(grantPermission?.Update?.Union(nowPermission?.Update), value?.Update)) return true;
					if (!CheckIfHavePermission(grantPermission?.Create?.Union(nowPermission?.Create), value?.Create)) return true;
					if (!CheckIfHavePermission(grantPermission?.Query?.Union(nowPermission?.Query), value?.Query)) return true;
					if (!CheckIfHavePermission(grantPermission?.Remove?.Union(nowPermission?.Remove), value?.Remove)) return true;
					return false;
				})) return false;
			}

			Update(permissions, newSerializeRaw);
			return true;
		}

		private static bool CheckIfHavePermission(IEnumerable<string> granter, IEnumerable<string> expectTo)
		{
			if (granter == null) return false;
			if (expectTo == null) return false;
			return expectTo.All(p => granter.Any(p.StartsWith));
		}

		private static void Update(this Permissions permissions, string newSerializeRaw) =>
			permissions.Regions = newSerializeRaw;

		/// <summary>
		/// 获取当前用户的权限域
		/// </summary>
		/// <param name="permissions"></param>
		/// <returns></returns>
		public static IDictionary<string, PermissionRegion> GetRegionList(this Permissions permissions)
		{
			var raw = permissions.Regions;
			if (raw == null) return new Dictionary<string, PermissionRegion>();
			return ToRegionList(raw);
		}

		public static IDictionary<string, PermissionRegion> ToRegionList(string raw)
		{
			return JsonConvert.DeserializeObject<IDictionary<string, PermissionRegion>>(raw ?? "{}") ?? new Dictionary<string, PermissionRegion>();
		}

		/// <summary>
		/// 获取指定权限列表
		/// </summary>
		/// <param name="permissions"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		private static PermissionRegion GetRegion(Permissions permissions, PermissionDescription key)
		{
			var dicList = permissions.GetRegionList();
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
			var dicList = permissions.GetRegionList();
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
				case Operation.Create: dic.Create = dic.Create.Append(targetUserCompanyCode); break;
				case Operation.Query: dic.Query = dic.Query.Append(targetUserCompanyCode); break;
				case Operation.Remove: dic.Remove = dic.Remove.Append(targetUserCompanyCode); break;
				case Operation.Update: dic.Update = dic.Update.Append(targetUserCompanyCode); break;
			}
			Update(permissions, JsonConvert.SerializeObject(dicList));
		}

		public static void Remove(this Permissions permissions, PermissionDescription key, Operation operation,
			string targetUserCompanyCode)
		{
			if (!Check(permissions, key, operation, targetUserCompanyCode)) return;
			var dicList = permissions.GetRegionList();
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
				case Operation.Create: dic.Create = dic.Create.Where(t => !t.StartsWith(targetUserCompanyCode)); break;
				case Operation.Query: dic.Query = dic.Query.Where(t => !t.StartsWith(targetUserCompanyCode)); break;
				case Operation.Remove: dic.Remove = dic.Remove.Where(t => !t.StartsWith(targetUserCompanyCode)); break;
				case Operation.Update: dic.Update = dic.Update.Where(t => !t.StartsWith(targetUserCompanyCode)); break;
			}
			Update(permissions, JsonConvert.SerializeObject(dicList));
		}

		public static bool Check(this Permissions permissions, PermissionDescription key, Operation operation, string targetUserCompanyCode)
		{
			if (permissions.Role.ToLower() == "admin") return true;
			var dic = GetRegion(permissions, key);
			if (dic == null) return false;
			IEnumerable<string> list;
			switch (operation)
			{
				case Operation.Create: list = dic.Create; break;
				case Operation.Query: list = dic.Query; break;
				case Operation.Remove: list = dic.Remove; break;
				case Operation.Update: list = dic.Update; break;
				default: return false;
			}
			if (targetUserCompanyCode == null) return list.Count() > 0;
			return list.Any(targetUserCompanyCode.StartsWith);
		}
	}

	public class PermissionRegion
	{
		public IEnumerable<string> Create { get; set; }
		public IEnumerable<string> Update { get; set; }
		public IEnumerable<string> Remove { get; set; }
		public IEnumerable<string> Query { get; set; }
	}
}