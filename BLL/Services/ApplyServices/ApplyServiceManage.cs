using BLL.Extensions;
using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using ExcelReport;
using ExcelReport.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService
	{
		public void RemoveAllUnSaveApply()
		{
			var list = _context.Applies
				.Where(a => a.Status == AuditStatus.NotSave)
				.Where(a=>a.Create.HasValue && a.Create.Value.AddDays(1).CompareTo(DateTime.Now)<0).ToList();
			if (list.Count == 0) return;
			foreach (var apply in list)
			{
				_context.ApplyResponses.RemoveRange(apply.Response);
			}
			_context.Applies.RemoveRange(list);
			_context.SaveChanges();
			var applies = _context.Applies;
			var request = _context.ApplyRequests.Where(r => !applies.Any(a => a.RequestInfo.Id == r.Id)).Where(r => DateTime.Now.Day != r.CreateTime.Day);
			_context.ApplyRequests.RemoveRange(request);
			var baseInfos = _context.ApplyBaseInfos.Where(r => !applies.Any(a => a.BaseInfo.Id == r.Id)).Where(r => DateTime.Now.Day != r.CreateTime.Day);
			_context.ApplyBaseInfos.RemoveRange(baseInfos);
			_context.SaveChanges();
		}

		public byte[] ExportExcel(string templete, string outPath, ApplyDetailDto model)
		{
			var sheetRenderers=new SheetRenderer[]
			{
				new SheetRenderer("Sheet1",
					new ParameterRenderer("RealName", model.Base?.RealName),//真实姓名不允许修改
					new ParameterRenderer("CompanyName", model.Company?.Name),
					new ParameterRenderer("StampLeave", model.RequestInfo?.StampLeave),
					new ParameterRenderer("StampReturn", model.RequestInfo?.StampReturn),
					new ParameterRenderer("HomeAddressDetail", model.Social?.AddressDetail),
					new ParameterRenderer("Phone", model.Social?.Phone),
					new ParameterRenderer("VocationType",model.RequestInfo?.VocationType),
					new ParameterRenderer("VocationDescription",model.RequestInfo.VocationDescription()),
					new ParameterRenderer("ByTransportation",model.RequestInfo?.ByTransportation),
					new ParameterRenderer("VocationLength",model.RequestInfo.VocationTotalLength()))
			};
			return Export.ExportToBuffer(templete, sheetRenderers);
		}

		public byte[] ExportExcel(string templete, string outPath, IEnumerable<ApplyDetailDto> model)
		{
			var list = model.ToList();
;			var sheetRenderers = new SheetRenderer[]
			{
				new SheetRenderer("Sheet1",
					new RepeaterRenderer<ApplyDetailDto>("List", list,
						new ParameterRenderer<ApplyDetailDto>("RequestInfo.VocationTotalLength", t => t.RequestInfo.VocationTotalLength()),
						new ParameterRenderer<ApplyDetailDto>("RequestInfo.VocationDescription", t => t.RequestInfo.VocationDescription()),
						new ParameterRenderer<ApplyDetailDto>("RequestInfo.OnTripLength", t => t.RequestInfo?.OnTripLength),
						new ParameterRenderer<ApplyDetailDto>("RequestInfo.StampLeave", t => t.RequestInfo?.StampLeave),
						new ParameterRenderer<ApplyDetailDto>("RequestInfo.StampReturn", t => t.RequestInfo?.StampReturn),
						new ParameterRenderer<ApplyDetailDto>("RequestInfo.VocationLength", t => t.RequestInfo?.VocationLength),
						new ParameterRenderer<ApplyDetailDto>("RequestInfo.VocationType", t => t.RequestInfo?.VocationType),
						new ParameterRenderer<ApplyDetailDto>("RequestInfo.ByTransportation", t => t.RequestInfo?.ByTransportation),
						new ParameterRenderer<ApplyDetailDto>("RequestInfo.CreateTime", t => t.RequestInfo?.CreateTime),
						new ParameterRenderer<ApplyDetailDto>("RequestInfo.Reason", t => t.RequestInfo?.Reason),
						new ParameterRenderer<ApplyDetailDto>("RequestInfo.Id", t => t.RequestInfo?.Id),
						new ParameterRenderer<ApplyDetailDto>("RequestInfo.VocationPlace", t => t.RequestInfo?.VocationPlace),
						new ParameterRenderer<ApplyDetailDto>("Base.Company", t => t.Base?.Company),
						new ParameterRenderer<ApplyDetailDto>("Base.Duties", t => t.Base?.Duties),
						new ParameterRenderer<ApplyDetailDto>("Base.RealName", t => t.Base?.RealName),
						new ParameterRenderer<ApplyDetailDto>("Base.Id", t => t.Base?.Id),
						new ParameterRenderer<ApplyDetailDto>("Company.Name", t => t.Company?.Name),
						new ParameterRenderer<ApplyDetailDto>("Company.Code", t => t.Company?.Code),
						new ParameterRenderer<ApplyDetailDto>("Status", t => t.Status),
						new ParameterRenderer<ApplyDetailDto>("Create", t => t.Create),
						new ParameterRenderer<ApplyDetailDto>("Duties.Name", t => t.Duties?.Name),
						new ParameterRenderer<ApplyDetailDto>("Social.Phone", t => t.Social?.Phone),
						new ParameterRenderer<ApplyDetailDto>("Social.Address.Name", t => t.Social?.Address?.Name),
						new ParameterRenderer<ApplyDetailDto>("Social.Address.Code", t => t.Social?.Address?.Code),
						new ParameterRenderer<ApplyDetailDto>("Social.Address.ShortName", t => t.Social?.Address?.ShortName),
						new ParameterRenderer<ApplyDetailDto>("Social.AddressDetail", t => t.Social?.AddressDetail),
						new ParameterRenderer<ApplyDetailDto>("Social.Settle", t => t.Social?.Settle),
						new ParameterRenderer<ApplyDetailDto>("Social.Id", t => t.Social?.Id),
						new ParameterRenderer<ApplyDetailDto>("Id", t => t.Id),
						new ParameterRenderer<ApplyDetailDto>("Response.SelfRankAudit",t=>t.Response?.SelfRankAuditStatus()),
						new ParameterRenderer<ApplyDetailDto>("Response.LastRankAudit",t=>t.Response?.LastRankAuditStatus()),
						new ParameterRenderer<ApplyDetailDto>("AuditLeader",t=>t.AuditLeader)
						),
					new ParameterRenderer("Author", "hzx")
				)
			};
			return Export.ExportToBuffer(templete, sheetRenderers);
		}
	}
}
