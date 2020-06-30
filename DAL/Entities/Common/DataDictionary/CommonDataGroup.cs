using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Common.DataDictionary
{
	public class CommonDataGroup : BaseEntityInt
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime Create { get; set; }
	}
}