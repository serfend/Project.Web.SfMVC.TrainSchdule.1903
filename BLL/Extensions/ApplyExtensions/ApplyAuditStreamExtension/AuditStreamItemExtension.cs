using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension
{

    public class AuditStreamModel : IHasGuidId, IAuditable
    {
        public ApplyAuditStreamSolutionRule ApplyAuditStreamSolutionRule { get; set; }
        public IEnumerable<ApplyAuditStep> ApplyAllAuditStep { get; set; }
        public string AuditLeader { get; set; }
        public ApplyAuditStep NowAuditStep { get; set; }
        public IEnumerable<ApplyResponse> Response { get; set; }
        public AuditStatus Status { get; set; }
        public Guid Id { get; set; }
    }
    public static class AuditStreamItemExtension
    {
        public static AuditStreamModel ToModel<T>(this T model, AuditStreamModel raw = null)where T: IHasGuidId, IAuditable,new()
        {
            if (raw == null) raw = new AuditStreamModel();
            raw.ApplyAllAuditStep = model.ApplyAllAuditStep;
            raw.ApplyAuditStreamSolutionRule = model.ApplyAuditStreamSolutionRule;
            raw.AuditLeader = model.AuditLeader;
            raw.NowAuditStep = model.NowAuditStep;
            raw.Response = model.Response;
            raw.Status = model.Status;
            raw.Id = model.Id;
            return raw;
        }
        public static T ToModel<T>(this AuditStreamModel model, T raw) where T : IHasGuidId, IAuditable, new()
        {
            if (raw == null) raw = new T();
            raw.ApplyAllAuditStep = model.ApplyAllAuditStep;
            raw.ApplyAuditStreamSolutionRule = model.ApplyAuditStreamSolutionRule;
            raw.AuditLeader = model.AuditLeader;
            raw.NowAuditStep = model.NowAuditStep;
            raw.Response = model.Response;
            raw.Status = model.Status;
            raw.Id = model.Id;
            return raw;
        }
    }
}