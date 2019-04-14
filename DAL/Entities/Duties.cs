using System;
using System.Collections.Generic;
using System.Text;
using TrainSchdule.DAL.Entities;

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
