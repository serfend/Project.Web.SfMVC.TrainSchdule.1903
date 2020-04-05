using BLL.Helpers;
using DAL.DTO.Apply.ApplyAuditStreamDTO;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DAL.DTO.Apply.ApplyAuditStreamDTO.ApplyAuditStreamSolutionRuleDto;

namespace TrainSchdule.ViewModels.Apply.ApplyAuditStream
{
	/// <summary>
	///
	/// </summary>
	public class StreamNodeListViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public StreamNodeListDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class StreamNodeListDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<ApplyAuditStreamNodeActionVDto> List { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class AuditStreamSolutionListViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public AuditStreamSolutionListDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class AuditStreamSolutionListDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<ApplyAuditStreamVDto> List { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class AuditStreamSolutionListRuleViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public AuditStreamSolutionListRuleDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class AuditStreamSolutionListRuleDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<ApplyAuditStreamSolutionRuleVDto> List { get; set; }
	}
}