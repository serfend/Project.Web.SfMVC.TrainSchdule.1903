using BLL.Helpers;
using DAL.DTO.Recall;
using System;
using System.ComponentModel.DataAnnotations;
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
	/// 恢复被删除的申请
	/// </summary>
	public class ApplyRestoreViewModel : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		public Guid Id { get; set; }
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
		/// 操作人id
		/// </summary>
		[Required]
		public string HandleBy { get; set; }

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
		/// 申请的id
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
		public static T ToVDto<T>(this RecallCreateDataModel model) where T : HandleByVdto, new()
		{
			if (model == null) return null;
			return new T()
			{
				Create = model.Create,
				ReturnStamp = model.ReturnStamp,
				Reason = model.Reason,
				Apply = model.Apply,
				HandleBy = new DAL.DTO.User.UserSummaryDto()
				{
					Id = model.HandleBy
				}
			};
		}
	}
}