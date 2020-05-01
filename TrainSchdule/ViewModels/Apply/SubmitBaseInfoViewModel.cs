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
		public string RealName { get; set; }

		/// <summary>
		/// 单位代码
		/// </summary>
		public string Company { get; set; }

		/// <summary>
		/// 职务名称
		/// </summary>
		public string Duties { get; set; }

		/// <summary>
		/// 家庭地址
		/// </summary>
		public int VocationTargetAddress { get; set; }

		/// <summary>
		///家庭地址（详细地址）
		/// </summary>
		public string VocationTargetAddressDetail { get; set; }

		/// <summary>
		/// 联系方式
		/// </summary>
		public string Phone { get; set; }

		/// <summary>
		/// 家庭情况
		/// </summary>
		public Settle Settle { get; set; }
	}
}