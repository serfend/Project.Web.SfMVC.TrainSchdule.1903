using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities.UserInfo;

namespace DAL.Entities
{
	public class Duties : BaseEntity
	{
		/// <summary>
		/// 职务的名称
		/// </summary>
		public string Name { get; set; }
	}
}
