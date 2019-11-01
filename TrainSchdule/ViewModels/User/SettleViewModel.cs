using DAL.Entities.UserInfo.Settle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 
	/// </summary>
	public class SettleDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public MomentDataModel Self { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public MomentDataModel Lover { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public MomentDataModel Parent { get; set; }
	}
	/// <summary>
	/// 
	/// </summary>
	public class MomentDataModel { 
		/// <summary>
		/// 
		/// </summary>
		public DateTime Date { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public bool Valid { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Address { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string AddressDetail { get; set; }
	}

}
