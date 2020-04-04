using DAL.Entities.UserInfo.Settle;
using System.ComponentModel.DataAnnotations;

namespace TrainSchdule.ViewModels.Apply
{
	/// <summary>
	///
	/// </summary>
	public class SubmitBaseInfoViewModel
	{
		/// <summary>
		///
		/// </summary>
		[Required]
		public string Id { get; set; }

		/// <summary>
		///
		/// </summary>
		[Required]
		public string RealName { get; set; }

		/// <summary>
		/// 单位代码
		/// </summary>
		[Required]
		public string Company { get; set; }

		/// <summary>
		/// 职务名称
		/// </summary>
		[Required]
		public string Duties { get; set; }

		/// <summary>
		/// 休假去向
		/// </summary>
		[Required]
		public int VocationTargetAddress { get; set; }

		/// <summary>
		///
		/// </summary>
		public string VocationTargetAddressDetail { get; set; }

		/// <summary>
		///
		/// </summary>
		public string Phone { get; set; }

		/// <summary>
		///
		/// </summary>
		public Settle Settle { get; set; }
	}
}