using DAL.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Vacations
{
	public class VacationType : BaseEntityInt, IRegion
	{
		/// <summary>
		/// 假期名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 别名
		/// </summary>
		public string Alias { get; set; }

		/// <summary>
		/// 最长长度
		/// </summary>
		public int MaxLength { get; set; }

		/// <summary>
		/// 最短长度
		/// </summary>
		public int MinLength { get; set; }

		/// <summary>
		/// 是否计入主假期
		/// </summary>
		public bool Primary { get; set; }

		/// <summary>
		/// 是否计算福利假
		/// </summary>
		public bool CaculateBenefit { get; set; }

		/// <summary>
		/// 是否可休路途
		/// </summary>
		public bool CanUseOnTrip { get; set; }

		/// <summary>
		/// TODO 当不计入当年时（非主假期）是否在次年减去
		/// </summary>
		public bool MinusNextYear { get; set; }

		/// <summary>
		/// 是否禁止跨年
		/// </summary>
		public bool NotPermitCrossYear { get; set; }

		public string RegionOnCompany { get; set; }
	}
}