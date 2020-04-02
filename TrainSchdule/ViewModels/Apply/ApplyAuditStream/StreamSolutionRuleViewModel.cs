using BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Apply.ApplyAuditStream
{
	/// <summary>
	///
	/// </summary>
	public class StreamSolutionRuleViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public StreamSolutionRuleDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class StreamSolutionRuleDataModel
	{
		/// <summary>
		///
		/// </summary>
		public DAL.Entities.ApplyInfo.ApplyAuditStreamSolutionRule Rule { get; set; }
	}
}