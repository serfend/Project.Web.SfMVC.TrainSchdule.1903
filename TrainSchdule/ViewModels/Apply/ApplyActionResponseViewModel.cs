using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using DAL.Entities.ApplyInfo;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Apply
{
	/// <summary>
	/// 返回操作结果
	/// </summary>
	public class ApplyActionResponseViewModel:ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="status"></param>
		public ApplyActionResponseViewModel(Status status)
		{
			this.Code = status.status;
			this.Message = status.message;
		}
		/// <summary>
		/// 
		/// </summary>
		public ApplyActionResponseDataModel Data { get; set; }
	}
	/// <summary>
	/// 
	/// </summary>
	public class ApplyActionResponseDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public AuditStatus Status { get; set; }
	}
}
