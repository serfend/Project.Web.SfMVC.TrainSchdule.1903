using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using DAL.Entities;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Static
{
	/// <summary>
	/// 
	/// </summary>
	public class VocationDescriptionViewModel:ApiResult
	{
		/// <summary>
		/// 
		/// </summary>
		public VocationDescriptionDataModel Data { get; set; }
	}
	/// <summary>
	/// 
	/// </summary>
	public class VocationDescriptionDataModel
	{
		/// <summary>
		/// 期间经历的节假日
		/// </summary>
		public IEnumerable<VocationDescription> Descriptions { get; set; }
		/// <summary>
		/// 休假中法定节假日天数
		/// </summary>
		public int VocationDays { get; set; }
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
