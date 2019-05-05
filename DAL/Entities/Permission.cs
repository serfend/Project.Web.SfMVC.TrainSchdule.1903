using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DAL.Entities
{
	public class Permissions:BaseEntity
	{
		public IDictionary<string, PermissionRegion> Regions { get; set; }
		public string OwnerId { get; set; }
		[ForeignKey(nameof(OwnerId))]
		public virtual UserInfo.User Owner { get; set; }
		
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
		public static bool Check(this Permissions permissions, PermissionDescription key,Operation operation,string targetUserCompanyCode)
		{
			var dic = permissions.Regions[key.Name];
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
			Entities.Company targetUserCompany) => Check(permissions, key, operation, targetUserCompany.Code);
		public static bool Check(this Permissions permissions, PermissionDescription key, Operation operation,
			Entities.UserInfo.User user) => Check(permissions, key, operation, user.CompanyInfo.Company.Code);
	}
	public class PermissionRegion:BaseEntity
	{
		public IEnumerable<string> Create { get; set; }
		public IEnumerable<string> Update { get; set; }
		public IEnumerable<string> Remove { get; set; }
		public IEnumerable<string> Query { get; set; }
	}

}
