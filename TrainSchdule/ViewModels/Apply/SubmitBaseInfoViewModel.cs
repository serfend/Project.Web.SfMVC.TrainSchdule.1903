using System.ComponentModel.DataAnnotations;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;

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
		public Settle Settle { get; set; }
	}



}
