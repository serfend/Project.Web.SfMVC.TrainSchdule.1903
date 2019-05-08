using System;
using System.ComponentModel.DataAnnotations;

namespace TrainSchdule.ViewModels.Apply
{
	/// <summary>
	/// 
	/// </summary>
	public class SubmitRequestInfoViewModel
	{
		/// <summary>
		/// 用户id
		/// </summary>
		[Required]
		public string Id { get; set; }
		/// <summary>
		/// 离队时间
		/// </summary>
		[Required]
		
		public DateTime?StampLeave { get; set; }
		/// <summary>
		/// 休假时长
		/// </summary>
		[Required]
		public int VocationLength { get; set; }
		/// <summary>
		/// 路途时间
		/// </summary>
		[Required]
		public int OnTripLength { get; set; }
		/// <summary>
		/// 休假类型
		/// </summary>
		[Required]
		public string VocationType { get; set; }
		/// <summary>
		/// 休假去向
		/// </summary>
		[Required]
		public int VocationPlace { get; set; }
		/// <summary>
		/// 休假原因
		/// </summary>
		public string Reason { get; set; }
	}
}
