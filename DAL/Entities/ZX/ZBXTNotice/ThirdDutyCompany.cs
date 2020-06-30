using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.ZX.ZBXTNotice
{
	public class ThirdDutyCompany : BaseEntityInt
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public virtual ThirdDutyCompany Parent { get; set; }
	}
}