using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Apply.ApplyAuditStream
{
	/// <summary>
	/// 审批节点创建
	/// </summary>
	public class StreamNodeCreateDataModel
	{
		/// <summary>
		/// 节点选择器
		/// </summary>
		public MembersFilter Filter { get; set; }

		/// <summary>
		/// 节点名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 节点描述
		/// </summary>
		public string Description { get; set; }
	}
}