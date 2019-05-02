using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrainSchdule.DAL.Entities.UserInfo.Permission
{
	public class PermissionRange
	{
		public IPermissionCheckable Create { get; set; }
		public IPermissionCheckable Query { get; set; }
		public IPermissionCheckable Remove { get; set; }
		public IPermissionCheckable Modify { get; set; }
	}

	public interface IPermissionCheckable
	{
		IEnumerable<PermittingAuth> PermittingAuths { get; set; }
		bool Check(string targetPath);
		bool Check(UserInfo.Company targetCompany);
		bool Check(UserInfo.User targetUser);
		bool Check();
	}

	public class PermittingAction:IPermissionCheckable
	{
		public IEnumerable<PermittingAuth> PermittingAuths { get; set; }
		public bool Check(string targetPath) => PermittingAuths.Any(p => targetPath.StartsWith(p.Path));
		public bool Check(UserInfo.Company targetCompany) => Check(targetCompany.Code);
		public bool Check(UserInfo.User targetUser) => Check(targetUser.Company.Code);
		public bool Check() => PermittingAuths.Any();
	}

	public class PermittingAuth
	{
		public string Path { get; set; }
		public Guid AuthBy { get; set; }
		public DateTime Create { get; set; }
	}
}
