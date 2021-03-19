using DAL.Entities.Common.DataDictionary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.ApplyInfo
{
	public class Apply : BaseEntityGuid,IAppliable
	{
		public virtual ApplyRequest RequestInfo { get; set; }
        public DateTime? Create { get ; set ; }
        public virtual ApplyBaseInfo BaseInfo { get ; set ; }
        public AuditStatus Status { get ; set ; }
        public string AuditLeader { get ; set ; }
        public MainStatus MainStatus { get ; set ; }
        public Guid? RecallId { get ; set ; }
        public virtual ApplyAuditStreamSolutionRule ApplyAuditStreamSolutionRule { get ; set ; }
		public virtual IEnumerable<ApplyAuditStep> ApplyAllAuditStep { get ; set ; }
        public virtual ApplyAuditStep NowAuditStep { get ; set ; }
        public virtual IEnumerable<ApplyResponse> Response { get ; set ; }
        public virtual ApplyExecuteStatus ExecuteStatusDetail { get; set; }
        public ExecuteStatus ExecuteStatus { get; set; }
        public Guid? ExecuteStatusDetailId { get; set; }
    }

	public class ActionByUserItem
	{
		public ActionByUserItem(string name, string alias, string type, string description)
		{
			Name = name;
			Alias = alias;
			Type = type;
			Description = description;
		}

		/// <summary>
		/// 按钮id
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 按钮名称
		/// </summary>
		public string Alias { get; set; }

		/// <summary>
		/// 按钮类型
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; }
	}
	public class AuditStatusMessage: EnmuDescriptionItem
	{
		public AuditStatusMessage() : base() { }
		public AuditStatusMessage(int code, string message, string desc, string color) : base(code, message, desc, color) { }
		/// <summary>
		/// 可进行的操作
		/// </summary>
		public IEnumerable<string> Acessable { get; set; }
	}
}