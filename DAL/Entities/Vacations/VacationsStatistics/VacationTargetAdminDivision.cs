using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Vacations.VacationsStatistics
{
	/// <summary>
	/// 休假地点，仅统计到省一级
	/// </summary>
	public class VacationTarget
	{
		public virtual AdminDivision AdminDivision { get; set; }
	}
}