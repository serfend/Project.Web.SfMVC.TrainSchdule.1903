using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.ApplyInfo.DailyApply;
using DAL.Entities.Vacations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTO.Apply
{

	public class ApplyRequestBaseVdto : IApplyRequestBase
    {
		public DateTime? StampLeave { get; set; }
		public DateTime? StampReturn { get; set; }

		public AdminDivision VacationPlace { get; set; }
		public string VacationPlaceName { get; set; }
		public string Reason { get; set; }
		public Transportation ByTransportation { get; set; }
	}

	public sealed class ApplyIndayRequestVdto: ApplyRequestBaseVdto {

		/// <summary>
		/// 请假类别
		/// </summary>
		public VacationIndayType RequestType { get; set; }
	}
	public sealed class ApplyRequestVdto: ApplyRequestBaseVdto
	{
		public VacationType VacationType { get; set; }
		public IEnumerable<VacationAdditional> VacationAdditionals { get; set; }
		/// <summary>
		/// 是否是计划休假
		/// </summary>
		public bool IsPlan { get; set; }
		/// <summary>
		/// 用户指定 id,length
		/// </summary>
		public Dictionary<int, int> LawVacationSet { get; set; }


		/// <summary>
		/// 本次休假中跨越的假期的描述
		/// </summary>
		public string VacationDescriptions { get; set; }

		public int OnTripLength { get; set; }
		public int VacationLength { get; set; }
	}
}