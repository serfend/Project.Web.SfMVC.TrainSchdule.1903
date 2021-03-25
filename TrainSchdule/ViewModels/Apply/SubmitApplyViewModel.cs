using System;
using System.ComponentModel.DataAnnotations;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Apply
{
	/// <summary>
	/// 
	/// </summary>
	public class SubmitApplyViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		[Required]
		public ScrollerVerifyViewModel Verify { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[Required]
		public Guid? BaseId { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[Required]
		public Guid? RequestId { get; set; }
		/// <summary>
		/// 是否是休假计划
		/// </summary>
		public bool IsPlan { get; set; }

		/// <summary>
		/// 审批流作用类型 可填写应用名称
		/// </summary>
		public string EntityType { get; set; }
	}

}
