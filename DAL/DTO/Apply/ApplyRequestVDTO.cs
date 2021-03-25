using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Vacations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.Apply
{
	public sealed class ApplyRequestVdto:IApplyRequestBase
	{
		public DateTime? StampLeave { get; set; }
		public DateTime? StampReturn { get; set; }

		/// <summary>
		/// 本次休假中跨越的假期的描述
		/// </summary>
		public string VacationDescriptions { get; set; }

		public int OnTripLength { get; set; }
		public int VacationLength { get; set; }
		public VacationType VacationType { get; set; }
		public AdminDivision VacationPlace { get; set; }
		public string VacationPlaceName { get; set; }
		public string Reason { get; set; }
		public Transportation ByTransportation { get; set; }
		public IEnumerable<VacationAdditional> VacationAdditionals { get; set; }
		/// <summary>
		/// 是否是计划休假
		/// </summary>
		public bool IsPlan { get; set; }
		/// <summary>
		/// 用户指定 id,length
		/// </summary>
		public Dictionary<int, int> LawVacationSet { get; set; }
	}
}