using BLL.Helpers;
using DAL.Entities;
using System;
using System.Collections.Generic;

namespace TrainSchdule.ViewModels.Static
{
	/// <summary>
	///
	/// </summary>
	public class VacationDescriptionViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public VacationDescriptionDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class VacationDescriptionDataModel
	{
		/// <summary>
		/// 期间经历的节假日
		/// </summary>
		public IEnumerable<VacationDescription> Descriptions { get; set; }

		/// <summary>
		/// 休假中法定节假日天数
		/// </summary>
		public int VacationDays { get; set; }

		/// <summary>
		/// 休假开始时间
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// 休假预计结束时间
		/// </summary>
		public DateTime EndDate { get; set; }
	}
}