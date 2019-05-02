using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSchdule.DAL.Entities.UserInfo.Permission
{
	public class Permissions:BaseEntity
	{
		public User User { get; set; }
		public Apply Apply { get; set; }
		public Company Company { get; set; }
		public virtual UserInfo.User Owner { get; set; }
	}



}
