using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TrainSchdule.DAL.Entities.UserInfo.Permission
{
	public class Permissions:BaseEntity
	{
		public virtual User User { get; set; }
		public virtual Apply Apply { get; set; }
		public virtual Company Company { get; set; }
		public string OwnerId { get; set; }
		[ForeignKey(nameof(OwnerId))]
		public virtual UserInfo.User Owner { get; set; }
	}



}
