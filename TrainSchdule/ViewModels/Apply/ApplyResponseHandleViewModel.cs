using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrainSchdule.ViewModels.Apply
{
	public class ApplyResponseHandleViewModel
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }
		[JsonProperty("apply")]
		public ApplyReponseHandleStatus Apply { get; set; }
		[JsonProperty("remark")]
		public string Remark { get; set; }
		[JsonProperty("auditAs")]
		public string AuditAs { get; set; }
	}

	public enum ApplyReponseHandleStatus
	{
		Accept=0,
		Deny=1
	}
}
