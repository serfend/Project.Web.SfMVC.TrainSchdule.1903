using BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities.ApplyInfo;
using DAL.DTO.Apply.ApplyAuditStreamDTO;

namespace TrainSchdule.ViewModels.Apply.ApplyAuditStream
{
	/// <summary>
	///
	/// </summary>
	public class StreamNodeViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public StreamNodeDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class StreamNodeDataModel
	{
		/// <summary>
		/// Node
		/// </summary>
		public ApplyAuditStreamNodeActionVDto Node { get; set; }
	}
}