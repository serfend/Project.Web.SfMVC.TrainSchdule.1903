
using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using ExcelReport;
using ExcelReport.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.DTO.Company;
using BLL.Extensions.ApplyExtensions;

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

		public byte[] ExportExcel(string templete, ApplyDetailDto model)
		{
			var sheetRenderers = new SheetRenderer[]
			{
				new SheetRenderer("Sheet1",SheetRenderer.ExtractModelToRender<ApplyDetailDto>(model).ToArray())
			};
			return Export.ExportToBuffer(templete, sheetRenderers);
		}

		public byte[] ExportExcel(string templete, IEnumerable<ApplyDetailDto> model,CompanyDto currentCompany)
		{
			var list = model.ToList();
			int index = 1;
			if (list.Count == 0) return null;
			IEmbeddedRenderer<ApplyDetailDto>[] parmList = {
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VocationTotalLength",
					t => t.RequestInfo.VocationTotalLength()),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VocationDescription",
					t => t.RequestInfo.VocationDescription()),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_OnTripLength", t => t.RequestInfo?.OnTripLength),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_StampLeave", t => t.RequestInfo?.StampLeave),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_StampReturn", t => t.RequestInfo?.StampReturn),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VocationLength", t => t.RequestInfo?.VocationLength),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VocationType", t => t.RequestInfo?.VocationType),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_ByTransportation",
					t => t.RequestInfo?.ByTransportation),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_CreateTime", t => t.RequestInfo?.CreateTime),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_Reason", t => t.RequestInfo?.Reason),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_Id", t => t.RequestInfo?.Id),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VocationPlace", t => t.RequestInfo?.VocationPlace.Name),
				new ParameterRenderer<ApplyDetailDto>("Base_Company", t => t.Base?.CompanyName),
				new ParameterRenderer<ApplyDetailDto>("Base_Duties", t => t.Base?.DutiesName),
				new ParameterRenderer<ApplyDetailDto>("Base_RealName", t => t.Base?.RealName),
				new ParameterRenderer<ApplyDetailDto>("Base_Id", t => t.Base?.Id),
				new ParameterRenderer<ApplyDetailDto>("Company_Name", t => t.Company?.Name),
				new ParameterRenderer<ApplyDetailDto>("Company_Code", t => t.Company?.Code),
				new ParameterRenderer<ApplyDetailDto>("Status", t => t.Status),
				new ParameterRenderer<ApplyDetailDto>("Create", t => t.Create),
				new ParameterRenderer<ApplyDetailDto>("Duties_Name", t => t.Duties?.Name),
				new ParameterRenderer<ApplyDetailDto>("Social_Phone", t => t.Social?.Phone),
				new ParameterRenderer<ApplyDetailDto>("Social_Address.Name", t => t.Social?.Address?.Name),
				new ParameterRenderer<ApplyDetailDto>("Social_Address.Code", t => t.Social?.Address?.Code),
				new ParameterRenderer<ApplyDetailDto>("Social_Address.ShortName", t => t.Social?.Address?.ShortName),
				new ParameterRenderer<ApplyDetailDto>("Social_AddressDetail", t => t.Social?.AddressDetail),
				new ParameterRenderer<ApplyDetailDto>("Social_Settle", t => t.Social?.Settle),
				new ParameterRenderer<ApplyDetailDto>("Social_Id", t => t.Social?.Id),
				new ParameterRenderer<ApplyDetailDto>("Id", t => t.Id),
				new ParameterRenderer<ApplyDetailDto>("Response_SelfRankAudit", t => t.Response?.SelfRankAuditStatus().AuditResult()),
				new ParameterRenderer<ApplyDetailDto>("Response_LastRankAudit", t => t.Response?.LastRankAuditStatus().AuditResult()),
				new ParameterRenderer<ApplyDetailDto>("AuditLeader", t => t.AuditLeader),
				new ParameterRenderer<ApplyDetailDto>("Index", t => index++)

			};
			return Export.ExportToBuffer(templete, new SheetRenderer("Sheet1",
				new RepeaterRenderer<ApplyDetailDto>("Roster", list, parmList),
				new ParameterRenderer("Audit_SelfCompanyName", "科/室"),
				new ParameterRenderer("Audit_HeadCompanyName","部")
			));
		}
	}
}
