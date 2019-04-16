using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSchdule.DAL.Entities
{
	public class PermissionCompany:BaseEntity
	{
		public string Path { get; set; }
		public virtual User Owner { get; set; }
		/// <summary>
		/// 授权人
		/// </summary>
		public Guid AuthBy { get; set; }
	}
}
