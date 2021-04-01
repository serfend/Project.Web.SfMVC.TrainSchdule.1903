using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using ExcelReport;
using ExcelReport.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.DTO.Company;
using BLL.Extensions.ApplyExtensions;
using BLL.Extensions;
using DAL.QueryModel;
using System.Threading.Tasks;
using BLL.Extensions.Common;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Expressions;
using DAL.Entities.UserInfo;
using DAL.Entities;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService
	{

		public byte[] ExportExcel(string templete, ApplyDetailDto<ApplyRequest> model)
		{
			if (model == null) return null;
			var list = SheetRenderer.ExtractModelToRender<ApplyDetailDto<ApplyRequest>>(model, (key, value) =>
			{
				switch (key)
				{
					default: return value;
				}
			}).ToList();
			list.Add(new ParameterRenderer("RequestInfo_VacationDescription", model.RequestInfo.RequestInfoVacationDescription()));
			list.Add(new ParameterRenderer("RequestInfo_VacationTotalLength", model.RequestInfo.VacationTotalLength()));
			list.Add(new ParameterRenderer("UserVacationInfo_DetailDescription", model.UserVacationDescription.VacationDescription()));
			list.Add(new ParameterRenderer("Social_IsMarried", model.Social.Settle.Lover.Valid ? "已婚" : "未婚"));

			var sheetRenderers = new SheetRenderer[]
				{
				new SheetRenderer("Sheet1",list.ToArray())
				};
			return Export.ExportToBuffer(templete, sheetRenderers);
		}

		public byte[] ExportExcel(string templete, IEnumerable<ApplyDetailDto<ApplyRequest>> model)
		{
			var list = model.ToList();
			int index = 1;
			if (list.Count == 0) return null;
			var mapList = new List<ParameterRenderer<ApplyDetailDto<ApplyRequest>>>()
			{
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("UserVacationInfo_LeftLength", t => t.UserVacationDescription?.LeftLength),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("UserVacationInfo_MaxTripTimes", t => t.UserVacationDescription?.MaxTripTimes),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("UserVacationInfo_NowTimes", t => t.UserVacationDescription?.NowTimes),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("UserVacationInfo_OnTripTimes", t => t.UserVacationDescription?.OnTripTimes),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("UserVacationInfo_YearlyLength", t => t.UserVacationDescription?.YearlyLength),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("UserVacationInfo_Description", t => t.UserVacationDescription?.Description),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("UserVacationInfo_VacationDescription", t => t.UserVacationDescription?.VacationDescription()),

				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("RequestInfo_VacationTotalLength",
					t => t.RequestInfo.VacationTotalLength()),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("RequestInfo_VacationDescription",
					t => t.RequestInfo.RequestInfoVacationDescription()),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("RequestInfo_OnTripLength", t => t.RequestInfo?.OnTripLength),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("RequestInfo_StampLeave", t => t.RequestInfo?.StampLeave),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("RequestInfo_StampReturn", t => t.RequestInfo?.StampReturn),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("RequestInfo_VacationLength", t => t.RequestInfo?.VacationLength),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("RequestInfo_VacationType", t => t.RequestInfo?.VacationType),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("RequestInfo_ByTransportation",
					t => t.RequestInfo?.ByTransportation),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("RequestInfo_CreateTime", t => t.RequestInfo?.CreateTime),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("RequestInfo_Reason", t => t.RequestInfo?.Reason),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("RequestInfo_Id", t => t.RequestInfo?.Id),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("RequestInfo_VacationPlace", t => t.RequestInfo?.VacationPlace.Name),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Base_Company", t => t.Base?.CompanyName),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Base_Duties", t => t.Base?.DutiesName),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Base_Title", t => t.Base?.UserTitle),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Base_RealName", t => t.Base?.RealName),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Base_Id", t => t.Base?.Id),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Company_Name", t => t.Company?.Name),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Company_Tag", t => t.Company?.Tag),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Company_Code", t => t.Company?.Code),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Status", t => t.Status),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Create", t => t.Create),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Duties_Name", t => t.Duties?.Name),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Social_Phone", t => t.Social?.Phone),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Social_Settle_Self_AddressDetail", t => t.Social?.Settle?.Self?.AddressDetail),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Social_Settle_Self_Address_Name", t => t.Social?.Settle?.Self?.Address?.Name),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Social_Id", t => t.Social?.Id),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Id", t => t.Id),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Response_SelfRankAudit", t => t.Response?.SelfRankAuditStatus().AuditResult()),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Response_LastRankAudit", t => t.Response?.LastRankAuditStatus().AuditResult()),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("AuditLeader", t => t.AuditLeader),
				new ParameterRenderer<ApplyDetailDto<ApplyRequest>>("Index", t => index++)
			};
			return Export.ExportToBuffer(templete, new SheetRenderer("Sheet1",
				new RepeaterRenderer<ApplyDetailDto<ApplyRequest>>("Roster", list, mapList.ToArray()),
				new ParameterRenderer("Audit_SelfCompanyName", "科/室"),
				new ParameterRenderer("Audit_HeadCompanyName", "部")
			));
		}
	}
}