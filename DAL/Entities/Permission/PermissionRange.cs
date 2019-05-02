using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrainSchdule.DAL.Entities.Permission
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
		bool Check(Entities.Company targetCompany);
		bool Check(Entities.User targetUser);
		bool Check();
	}

	public class PermittingAction:IPermissionCheckable
	{
		public IEnumerable<PermittingAuth> PermittingAuths { get; set; }
		public bool Check(string targetPath) => PermittingAuths.Any(p => targetPath.StartsWith(p.Path));
		public bool Check(Entities.Company targetCompany) => Check(targetCompany.Code);
		public bool Check(Entities.User targetUser) => Check(targetUser.Company.Code);
		public bool Check() => PermittingAuths.Any();
	}

	public class PermittingAuth
	{
		public string Path { get; set; }
		public Guid AuthBy { get; set; }
		public DateTime Create { get; set; }
	}
}
