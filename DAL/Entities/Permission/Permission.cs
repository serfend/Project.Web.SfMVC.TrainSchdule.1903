using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSchdule.DAL.Entities.Permission
{
	public class Permissions:BaseEntity
	{
		public User User { get; set; }
		public Apply Apply { get; set; }
		public Company Company { get; set; }
		public virtual TrainSchdule.DAL.Entities.User Owner { get; set; }
	}



}
