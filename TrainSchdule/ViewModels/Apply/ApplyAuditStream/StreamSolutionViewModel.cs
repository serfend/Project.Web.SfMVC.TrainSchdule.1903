using BLL.Helpers;
using DAL.DTO.Apply.ApplyAuditStreamDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Apply.ApplyAuditStream
{
	/// <summary>
	///
	/// </summary>
	public class StreamSolutionViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public StreamSolutionDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class StreamSolutionDataModel
	{
		/// <summary>
		///
		/// </summary>
		public ApplyAuditStreamVDto Solution { get; set; }
	}
}