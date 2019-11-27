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
	public class RecallViewModel : ApiDataModel
	{
		public RecallOrderVDto Data { get; set; }
	}
	public class RecallCreateViewModel : GoogleAuthViewModel
	{
		public RecallCreateDataModel Data { get; set; }
	}
	public class RecallCreateDataModel
	{

		public string Reason { get; set; }
		/// <summary>
		/// 召回人id
		/// </summary>
		[Required]
		public string RecallBy { get; set; }
		public DateTime Create { get; set; }
		[Required]

		public DateTime ReturnStamp { get; set; }
		/// <summary>
		/// 召回的申请的id
		/// </summary>
		[Required]
		public Guid Apply { get; set; }
	}
	public static class RecallOrderExtensions
	{
		public static RecallOrderVDto ToVDto(this RecallCreateDataModel model)
		{
			if (model == null) return null;
			return new RecallOrderVDto()
			{
				Create = model.Create,
				ReturnStamp = model.ReturnStamp,
				Reason = model.Reason,
				Apply =model.Apply,
				RecallBy=new DAL.DTO.User.UserSummaryDto()
				{
					Id=model.RecallBy
				}
			};
		}
	}
}
