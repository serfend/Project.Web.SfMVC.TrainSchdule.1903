using BLL.Helpers;
using DAL.DTO.Apply;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Apply
{
	/// <summary>
	/// 
	/// </summary>
	public class InfoApplyDetailViewModel : ApiResult
	{
		/// <summary>
		/// 
		/// </summary>
		public ApplyDetailDto Data { get; set; }
	}

}
