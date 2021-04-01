using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Abp.Linq.Expressions;
using Castle.Core.Internal;
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
		[Description("用户")]
		public static class User
		{
			public static PermissionDescription Application = new PermissionDescription("User.Application", "用户的密码、安全码等敏感信息");
			public static PermissionDescription BaseInfo = new PermissionDescription("User.BaseInfo", "用户的基本信息");
			public static PermissionDescription SocialInfo = new PermissionDescription("User.SocialInfo", "用户的家庭情况信息");
		}

		[Description("单位")]
		public static class Company
		{
			public static PermissionDescription View = new PermissionDescription("Company.View", "单位视图信息");
		}

		[Description("休假")]
		public static class Apply
		{
			public static PermissionDescription Default = new PermissionDescription("Apply.Default", "休假申请通用");
			public static PermissionDescription AuditStream = new PermissionDescription("Apply.AuditStream", "申请的审批流");
			public static PermissionDescription AttachInfo = new PermissionDescription("Apply.AttachInfo", "申请的附加信息");
			public static PermissionDescription InDayApply = new PermissionDescription("Apply.Default", "请假申请通用");
		}
		[Description("成绩")]
		public static class Grade
		{
			public static PermissionDescription Subject = new PermissionDescription("Grade.Subject", "成绩-科目标准");
			public static PermissionDescription Exam = new PermissionDescription("Grade.Exam", "成绩-考核");
			public static PermissionDescription Record = new PermissionDescription("Grade.Record", "成绩-记录");
			public static PermissionDescription MemberRate = new PermissionDescription("Grade.MemberRate", "成绩-评比与考核成绩");
		}

		[Description("资源")]
		public static class Resources
		{
			public static PermissionDescription Default = new PermissionDescription("Resources.Default", "常规资源");
			public static PermissionDescription ShortUrl = new PermissionDescription("Resources.ShortUrl", "短网址");
		}
		[Description("动态")]
		public static class Post
        {
			public static PermissionDescription Default = new PermissionDescription("Post.Default","常规动态");
			public static PermissionDescription AppMessage = new PermissionDescription("Post.AppMessage","用户消息");
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
		/// <param name="grantCompany">授权单位</param>
		/// <returns></returns>
		public static bool Update(this Permissions permissions, string newSerializeRaw, Permissions grantBy, IEnumerable<string> grantCompanies)
		{
			var result = CheckPermissionAuth(permissions, newSerializeRaw, grantBy, grantCompanies);
			if (!result) return false;
			Update(permissions, newSerializeRaw);
			return true;
		}

		public static bool CheckPermissionAuth(this Permissions permissions, string newSerializeRaw, Permissions grantBy, IEnumerable<string> grantCompanies)
        {
			if (grantBy.Role.ToLower() != "admin")
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
					if (!CheckIfHavePermission(grantPermission?.Update?.Union(nowPermission?.Update), value?.Update, grantCompanies)) return true;
					if (!CheckIfHavePermission(grantPermission?.Create?.Union(nowPermission?.Create), value?.Create, grantCompanies)) return true;
					if (!CheckIfHavePermission(grantPermission?.Query?.Union(nowPermission?.Query), value?.Query, grantCompanies)) return true;
					if (!CheckIfHavePermission(grantPermission?.Remove?.Union(nowPermission?.Remove), value?.Remove, grantCompanies)) return true;
					return false;
				})) return false;
			}
			return true;
		}
		/// <summary>
		/// 检查是否有权限
		/// </summary>
		/// <param name="granter"></param>
		/// <param name="expectTo"></param>
		/// <param name="grantCompany">此单位下直接授权</param>
		/// <returns></returns>
		private static bool CheckIfHavePermission(IEnumerable<string> granter, IEnumerable<string> expectTo,IEnumerable<string> grantCompanies)
		{
			if (expectTo == null) return false;
			var exp = PredicateBuilder.New<string>(false);
			if (granter != null) exp = exp.Or(p => granter.Any(p.StartsWith));
			if (grantCompanies != null) exp = exp.Or(p=>grantCompanies.Any(p.StartsWith));
			return expectTo.All(exp);
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
			var raw = permissions?.Regions;
			if (raw == null) return new Dictionary<string, PermissionRegion>();
			return ToRegionList(raw);
		}

		public static IDictionary<string, PermissionRegion> ToRegionList(string raw)
		{
			if (raw.IsNullOrEmpty()) raw = "{}";
			var r = JsonConvert.DeserializeObject<IDictionary<string, PermissionRegion>>(raw);
			return r ?? new Dictionary<string, PermissionRegion>();
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
			return list?.Any(targetUserCompanyCode.StartsWith) ?? false;
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