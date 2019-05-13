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

		public byte[] ExportExcel(string templete, ApplyDetailDto model)
		{
			var sheetRenderers=new SheetRenderer[]
			{
				new SheetRenderer("Sheet1",
					new ParameterRenderer("RequestInfo_VocationTotalLength",
					model.RequestInfo.VocationTotalLength()),
				new ParameterRenderer("RequestInfo_VocationDescription",
					model.RequestInfo.VocationDescription()),
				new ParameterRenderer("RequestInfo_OnTripLength", model.RequestInfo?.OnTripLength),
				new ParameterRenderer("RequestInfo_StampLeave", model.RequestInfo?.StampLeave),
				new ParameterRenderer("RequestInfo_StampReturn", model.RequestInfo?.StampReturn),
				new ParameterRenderer("RequestInfo_VocationLength", model.RequestInfo?.VocationLength),
				new ParameterRenderer("RequestInfo_VocationType", model.RequestInfo?.VocationType),
				new ParameterRenderer("RequestInfo_ByTransportation",
					model.RequestInfo?.ByTransportation),
				new ParameterRenderer("RequestInfo_CreateTime", model.RequestInfo?.CreateTime),
				new ParameterRenderer("RequestInfo_Reason", model.RequestInfo?.Reason),
				new ParameterRenderer("RequestInfo_Id", model.RequestInfo?.Id),
				new ParameterRenderer("RequestInfo_VocationPlace", model.RequestInfo?.VocationPlace.Name),
				new ParameterRenderer("Base_Company", model.Base?.Company),
				new ParameterRenderer("Base_Duties", model.Base?.Duties),
				new ParameterRenderer("Base_RealName", model.Base?.RealName),
				new ParameterRenderer("Base_Id", model.Base?.Id),
				new ParameterRenderer("Company_Name", model.Company?.Name),
				new ParameterRenderer("Company_Code", model.Company?.Code),
				new ParameterRenderer("Status", model.Status),
				new ParameterRenderer("Create", model.Create),
				new ParameterRenderer("Duties_Name", model.Duties?.Name),
				new ParameterRenderer("Social_Phone", model.Social?.Phone),
				new ParameterRenderer("Social_Address.Name", model.Social?.Address?.Name),
				new ParameterRenderer("Social_Address.Code", model.Social?.Address?.Code),
				new ParameterRenderer("Social_Address.ShortName", model.Social?.Address?.ShortName),
				new ParameterRenderer("Social_AddressDetail", model.Social?.AddressDetail),
				new ParameterRenderer("Social_Settle", model.Social?.Settle),
				new ParameterRenderer("Social_Id", model.Social?.Id),
				new ParameterRenderer("Id", model.Id),
				new ParameterRenderer("Response_SelfRankAudit", model.Response?.SelfRankAuditStatus().AuditResult()),
				new ParameterRenderer("Response_LastRankAudit", model.Response?.LastRankAuditStatus().AuditResult()),
				new ParameterRenderer("AuditLeader", model.AuditLeader)
					),
					
			};
			return Export.ExportToBuffer(templete, sheetRenderers);
		}

		public byte[] ExportExcel(string templete, IEnumerable<ApplyDetailDto> model)
		{
			var list = model.ToList();
			int index = 1;
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
				new ParameterRenderer<ApplyDetailDto>("Base_Company", t => t.Base?.Company),
				new ParameterRenderer<ApplyDetailDto>("Base_Duties", t => t.Base?.Duties),
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
				new RepeaterRenderer<ApplyDetailDto>("Roster", list, parmList)
			));
		}
	}
}
