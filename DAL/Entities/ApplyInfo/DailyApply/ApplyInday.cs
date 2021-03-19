using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ApplyInfo.DailyApply
{
    /// <summary>
    /// 日请假
    /// </summary>
    public class ApplyInday : BaseEntityGuid, IAppliable, IAuditable, IExecutable
    {
        public virtual ApplyIndayRequest RequestInfo {get;set;}
        public ApplyBaseInfo BaseInfo { get ; set ; }
        public DateTime? Create { get ; set ; }
        public AuditStatus Status { get ; set ; }
        public string AuditLeader { get ; set ; }
        public Guid? RecallId { get ; set ; }
        public MainStatus MainStatus { get ; set ; }
        public ExecuteStatus ExecuteStatus { get ; set ; }
        [ForeignKey("ExecuteStatusDetailId")]
        public virtual ApplyExecuteStatus ExecuteStatusDetail { get; set; }
        public Guid? ExecuteStatusDetailId { get ; set ; }
        public ApplyAuditStreamSolutionRule ApplyAuditStreamSolutionRule { get ; set ; }
        public IEnumerable<ApplyAuditStep> ApplyAllAuditStep { get ; set ; }
        public ApplyAuditStep NowAuditStep { get ; set ; }
        public IEnumerable<ApplyResponse> Response { get ; set ; }
    }
}
