using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.ZX.ZBXTNotice
{
	public class ThirdDutyUser : BaseEntityInt
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public virtual ThirdDutyCompany Company { get; set; }
	}
}