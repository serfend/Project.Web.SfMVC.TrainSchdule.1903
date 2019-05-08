using System.ComponentModel.DataAnnotations;
using DAL.Entities.UserInfo;

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
		/// 
		/// </summary>
		[Required]
		public string Company { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[Required]
		public string Duties { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[Required]
		public int HomeAddress { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string HomeDetailAddress { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Phone { get; set; }
		/// <summary>
		/// 不符合随军,
		/// 符合随军未随军同地,
		/// 符合随军未随军异地,
		/// 已随军,
		/// 双军人同地,
		/// 双军人异地
		/// </summary>
		public SettleDownEnum Settle { get; set; }
	}



}
