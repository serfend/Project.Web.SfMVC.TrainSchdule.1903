using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 用户休假信息
	/// </summary>
	public class UserVocationInfoViewModel : ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public UserVocationInfoDataModel Data{get;set;}
	}
	/// <summary>
	/// 
	/// </summary>
	public class UserVocationInfoDataModel
	{
		/// <summary>
		/// 全年总天数
		/// </summary>
		public int YearlyLength { get; set; }
		/// <summary>
		/// 当前已休假次数
		/// </summary>
		public int NowTimes { get; set; }
		/// <summary>
		/// 剩休假天数
		/// </summary>
		public int LeftLength { get; set; }
		/// <summary>
		/// 已休路途次数
		/// </summary>
		public int OnTripTimes { get; set; }
		public int MaxTripTimes { get; set; }
	}


}
