using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrainSchdule.BLL.Helpers;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Apply
{
	/// <summary>
	/// 需要审核的流程
	/// </summary>
	public class ApplyResponseHandleViewModel
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }
		[JsonProperty("apply")]
		public ApplyReponseHandleStatus Apply { get; set; }
		[JsonProperty("remark")]
		public string Remark { get; set; }
	}
	/// <summary>
	/// 处理审核结果
	/// </summary>
	public class ApplyResponseHandledViewModel:APIDataModel
	{
		public Dictionary<string, ApplyResponseHandledDataModel> Data { get; set; }
	}

	public class ApplyResponseHandledDataModel : APIDataModel
	{
		public ApplyResponseHandledDataModel(Status status)
		{
			this.Code = status.status;
			this.Message = status.message;
		}
	}
	public enum ApplyReponseHandleStatus
	{
		Accept=0,
		Deny=1
	}
}
