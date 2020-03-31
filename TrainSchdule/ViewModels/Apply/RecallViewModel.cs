using BLL.Helpers;
using DAL.DTO.Recall;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Apply
{
	/// <summary>
	///
	/// </summary>
	public class RecallViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public RecallOrderVDto Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class RecallCreateViewModel : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		public RecallCreateDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class RecallCreateDataModel
	{
		/// <summary>
		///
		/// </summary>
		public string Reason { get; set; }

		/// <summary>
		/// 召回人id
		/// </summary>
		[Required]
		public string RecallBy { get; set; }

		/// <summary>
		///
		/// </summary>
		public DateTime Create { get; set; }

		/// <summary>
		///
		/// </summary>
		[Required]
		public DateTime ReturnStamp { get; set; }

		/// <summary>
		/// 召回的申请的id
		/// </summary>
		[Required]
		public Guid Apply { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public static class RecallOrderExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static RecallOrderVDto ToVDto(this RecallCreateDataModel model)
		{
			if (model == null) return null;
			return new RecallOrderVDto()
			{
				Create = model.Create,
				ReturnStamp = model.ReturnStamp,
				Reason = model.Reason,
				Apply = model.Apply,
				RecallBy = new DAL.DTO.User.UserSummaryDto()
				{
					Id = model.RecallBy
				}
			};
		}
	}
}