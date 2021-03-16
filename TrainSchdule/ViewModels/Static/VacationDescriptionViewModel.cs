using BLL.Helpers;
using DAL.DTO.Apply;
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
	/// 用户传入，检查归队时间
	/// </summary>
	public class VacationDateCheckDataModel
	{
		/// <summary>
		/// 开始日期
		/// </summary>
		public DateTime Start { get; set; }
		/// <summary>
		/// 长度
		/// </summary>
		public int Length { get; set; }
		/// <summary>
		/// 是否计算法定节假日
		/// </summary>
		public bool CaculateLawVacation { get; set; }
		/// <summary>
		/// 福利假长度（不计算法定节假日天数）
		/// </summary>
		public int Benefits { get; set; }
		/// <summary>
		/// 指定福利假的用户范围
		/// </summary>
		public Dictionary<int,int> LawVacationSet { get; set; }
	}
	/// <summary>
	///
	/// </summary>
	public class VacationDescriptionDataModel
	{
		/// <summary>
		/// 期间经历的节假日
		/// </summary>
		public IEnumerable<VacationDescriptionDto> Descriptions { get; set; }

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