using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrainSchdule.DAL.Entities.UserInfo.Permission
{
	public class PermissionRange : BaseEntity
	{
		public virtual PermittingAction Create { get; set; }
		public virtual PermittingAction Query { get; set; }
		public virtual PermittingAction Remove { get; set; }
		public virtual PermittingAction Modify { get; set; }
	}


	public class PermittingAction : BaseEntity
	{
		public virtual IEnumerable<PermittingAuth> PermittingAuths { get; set; }
		public bool Check(string targetPath) => PermittingAuths.Any(p => targetPath.StartsWith(p.Path));
		public bool Check(UserInfo.Company targetCompany) => Check(targetCompany.Code);
		public bool Check(UserInfo.User targetUser) => Check(targetUser.Company.Code);
		public bool Check() => PermittingAuths.Any();
	}

	public class PermittingAuth : BaseEntity
	{
		public string Path { get; set; }
		public Guid AuthBy { get; set; }
		public DateTime Create { get; set; }
	}
}
