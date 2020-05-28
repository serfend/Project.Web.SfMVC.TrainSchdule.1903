﻿using DAL.DTO.Apply;
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

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService
	{
		public IEnumerable<Apply> QueryApplies(QueryApplyDataModel model, bool getAllAppliesPermission, out int totalCount)
		{
			totalCount = 0;
			var list = _context.AppliesDb;
			if (model == null) return null;
			if (model.Status != null) list = list.Where(a => (model.Status.Arrays != null && model.Status.Arrays.Contains((int)a.Status)) || (model.Status.Start <= (int)a.Status && model.Status.End >= (int)a.Status));

			if (model.NowAuditBy != null) list = list.Where(a => a.NowAuditStep.MembersFitToAudit.Contains(model.NowAuditBy.Value));
			if (model.AuditBy != null) list = list.Where(a => a.ApplyAllAuditStep.Any(s => s.MembersFitToAudit.Contains(model.AuditBy.Value)));

			if (model.CreateCompany != null) list = list.Where(a => a.BaseInfo.From.CompanyInfo.Company.Code == model.CreateCompany.Value);

			bool anyDateFilterIsLessThan30Days = false;
			if (model.Create != null)
			{
				list = list.Where(a => (a.Create >= model.Create.Start && a.Create <= model.Create.End) || (model.Create.Dates != null && model.Create.Dates.Any(d => d.Date.Subtract(a.Create.Value).Days == 0)));
				anyDateFilterIsLessThan30Days |= model.Create.End.Subtract(model.Create.Start).Days <= 360;
			}

			////  默认查询到下周六前的的申请
			//if (model.StampLeave == null)
			//{
			//	var thisFri = DayOfWeek.Friday;
			//	var nowDay = DateTime.Now;
			//	model.StampLeave = new QueryByDate()
			//	{
			//		Start = nowDay,
			//		End = nowDay.AddDays(thisFri - nowDay.DayOfWeek).AddDays(8)
			//	};
			//}
			if (model.StampLeave?.Start != null) list = list.Where(a => (a.RequestInfo.StampLeave >= model.StampLeave.Start && a.RequestInfo.StampLeave <= model.StampLeave.End) || (model.StampLeave.Dates != null && model.StampLeave.Dates.Any(d => d.Date.Subtract(a.RequestInfo.StampLeave.Value).Days == 0)));
			anyDateFilterIsLessThan30Days |= (model.StampLeave == null || model.StampLeave.End.Subtract(model.StampLeave.Start).Days <= 360);

			if (model.StampReturn != null)
			{
				list = list.Where(a => (a.RequestInfo.StampReturn >= model.StampReturn.Start && a.RequestInfo.StampReturn <= model.StampReturn.End) || (model.StampReturn.Dates != null && model.StampReturn.Dates.Any(d => d.Date.Subtract(a.RequestInfo.StampReturn.Value).Days == 0)));
				anyDateFilterIsLessThan30Days |= model.StampReturn.End.Subtract(model.StampReturn.Start).Days <= 360;
			}
			if (!getAllAppliesPermission && !anyDateFilterIsLessThan30Days) list = list.Where(a => a.RequestInfo.StampLeave >= new DateTime(DateTime.Now.Year, 1, 1)); //默认返回今年以来所有假期

			// 若精确按id或按人查询，则直接导出
			if (model.CreateBy != null)
			{
				list = _context.AppliesDb.Where(a => a.BaseInfo.CreateBy.Id == model.CreateBy.Value || a.BaseInfo.CreateBy.BaseInfo.RealName == (model.CreateBy.Value));
			}
			else if (model.CreateFor != null)
			{
				list = _context.AppliesDb.Where(a => a.BaseInfo.From.Id == model.CreateFor.Value || a.BaseInfo.From.BaseInfo.RealName == (model.CreateFor.Value));
			}
			list = list.OrderByDescending(a => a.Create).ThenByDescending(a => a.Status);
			if (model.Pages == null || model.Pages.PageIndex < 0 || model.Pages.PageSize <= 0) model.Pages = new QueryByPage()
			{
				PageIndex = 0,
				PageSize = 20
			};
			var result = list.SplitPage(model.Pages).Result;
			totalCount = result.Item2;
			return result.Item1;
		}

		public async Task RemoveAllUnSaveApply()
		{
			//寻找所有找过1天未保存的申请
			var list = _context.AppliesDb
						 .Where(a => a.Status == AuditStatus.NotSave)
						 .Where(a => a.Create.HasValue && a.Create.Value.AddDays(1).Subtract(DateTime.Now).TotalDays < 0).ToList();
			await RemoveApplies(list).ConfigureAwait(true);
		}

		public async Task RemoveAllNoneFromUserApply()
		{
			var applies = _context.AppliesDb;

			#region request

			//寻找所有没有创建申请且不是今天创建的 请求信息
			var request = _context.ApplyRequests.Where(r => DateTime.Now.Day != r.CreateTime.Day).Where(r => !applies.Any(a => a.RequestInfo.Id == r.Id)).ToList();
			//删除这些请求信息的福利信息
			foreach (var r in request) _context.VacationAdditionals.RemoveRange(r.AdditialVacations);
			//删除这些请求信息
			_context.ApplyRequests.RemoveRange(request);

			#endregion request

			#region steps

			// 删除所有无申请指向的步骤
			var applySteps = _context.ApplyAuditSteps.Where(s => !_context.AppliesDb.Any(a => a.ApplyAllAuditStep.Any(step => step.Id == s.Id)));
			_context.ApplyAuditSteps.RemoveRange(applySteps);

			#endregion steps

			#region base

			//寻找所有没有创建申请且不是今天创建的 基础信息
			var baseInfos = _context.ApplyBaseInfos.Where(r => DateTime.Now.Day != r.CreateTime.Day).Where(r => !applies.Any(a => a.BaseInfo.Id == r.Id));
			//删除这些基础信息
			_context.ApplyBaseInfos.RemoveRange(baseInfos);

			#endregion base

			#region response

			//寻找所有没有被引用了的反馈信息
			var responses = _context.ApplyResponses.Where(r => !applies.Any(a => a.Response.Any(ar => ar.Id == r.Id)));
			_context.ApplyResponses.RemoveRange(responses);

			#endregion response

			var list = _context.AppliesDb.Where(a => a.BaseInfo.From == null);
			await RemoveApplies(list).ConfigureAwait(true);
		}

		public async Task RemoveApplies(IEnumerable<Apply> list)
		{
			if (list == null) return;
			bool anyRemove = false;
			foreach (var s in list)
			{
				s.Remove();
				_context.Applies.Update(s);
				anyRemove = true;
			}
			if (anyRemove)
				await _context.SaveChangesAsync().ConfigureAwait(true);
		}

		public byte[] ExportExcel(string templete, ApplyDetailDto model)
		{
			if (model == null) return null;
			var list = SheetRenderer.ExtractModelToRender<ApplyDetailDto>(model, (key, value) =>
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

		public byte[] ExportExcel(string templete, IEnumerable<ApplyDetailDto> model)
		{
			var list = model.ToList();
			int index = 1;
			if (list.Count == 0) return null;
			var mapList = new List<ParameterRenderer<ApplyDetailDto>>()
			{
				new ParameterRenderer<ApplyDetailDto>("UserVacationInfo_LeftLength", t => t.UserVacationDescription?.LeftLength),
				new ParameterRenderer<ApplyDetailDto>("UserVacationInfo_MaxTripTimes", t => t.UserVacationDescription?.MaxTripTimes),
				new ParameterRenderer<ApplyDetailDto>("UserVacationInfo_NowTimes", t => t.UserVacationDescription?.NowTimes),
				new ParameterRenderer<ApplyDetailDto>("UserVacationInfo_OnTripTimes", t => t.UserVacationDescription?.OnTripTimes),
				new ParameterRenderer<ApplyDetailDto>("UserVacationInfo_YearlyLength", t => t.UserVacationDescription?.YearlyLength),
				new ParameterRenderer<ApplyDetailDto>("UserVacationInfo_Description", t => t.UserVacationDescription?.Description),
				new ParameterRenderer<ApplyDetailDto>("UserVacationInfo_VacationDescription", t => t.UserVacationDescription?.VacationDescription()),

				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VacationTotalLength",
					t => t.RequestInfo.VacationTotalLength()),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VacationDescription",
					t => t.RequestInfo.RequestInfoVacationDescription()),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_OnTripLength", t => t.RequestInfo?.OnTripLength),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_StampLeave", t => t.RequestInfo?.StampLeave),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_StampReturn", t => t.RequestInfo?.StampReturn),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VacationLength", t => t.RequestInfo?.VacationLength),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VacationType", t => t.RequestInfo?.VacationType),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_ByTransportation",
					t => t.RequestInfo?.ByTransportation),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_CreateTime", t => t.RequestInfo?.CreateTime),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_Reason", t => t.RequestInfo?.Reason),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_Id", t => t.RequestInfo?.Id),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VacationPlace", t => t.RequestInfo?.VacationPlace.Name),
				new ParameterRenderer<ApplyDetailDto>("Base_Company", t => t.Base?.CompanyName),
				new ParameterRenderer<ApplyDetailDto>("Base_Duties", t => t.Base?.DutiesName),
				new ParameterRenderer<ApplyDetailDto>("Base_Title", t => t.Base?.UserTitle),
				new ParameterRenderer<ApplyDetailDto>("Base_RealName", t => t.Base?.RealName),
				new ParameterRenderer<ApplyDetailDto>("Base_Id", t => t.Base?.Id),
				new ParameterRenderer<ApplyDetailDto>("Company_Name", t => t.Company?.Name),
				new ParameterRenderer<ApplyDetailDto>("Company_Tag", t => t.Company?.Tag),
				new ParameterRenderer<ApplyDetailDto>("Company_Code", t => t.Company?.Code),
				new ParameterRenderer<ApplyDetailDto>("Status", t => t.Status),
				new ParameterRenderer<ApplyDetailDto>("Create", t => t.Create),
				new ParameterRenderer<ApplyDetailDto>("Duties_Name", t => t.Duties?.Name),
				new ParameterRenderer<ApplyDetailDto>("Social_Phone", t => t.Social?.Phone),
				new ParameterRenderer<ApplyDetailDto>("Social_Settle_Self_AddressDetail", t => t.Social?.Settle?.Self?.AddressDetail),
				new ParameterRenderer<ApplyDetailDto>("Social_Settle_Self_Address_Name", t => t.Social?.Settle?.Self?.Address?.Name),
				new ParameterRenderer<ApplyDetailDto>("Social_Id", t => t.Social?.Id),
				new ParameterRenderer<ApplyDetailDto>("Id", t => t.Id),
				new ParameterRenderer<ApplyDetailDto>("Response_SelfRankAudit", t => t.Response?.SelfRankAuditStatus().AuditResult()),
				new ParameterRenderer<ApplyDetailDto>("Response_LastRankAudit", t => t.Response?.LastRankAuditStatus().AuditResult()),
				new ParameterRenderer<ApplyDetailDto>("AuditLeader", t => t.AuditLeader),
				new ParameterRenderer<ApplyDetailDto>("Index", t => index++)
			};
			return Export.ExportToBuffer(templete, new SheetRenderer("Sheet1",
				new RepeaterRenderer<ApplyDetailDto>("Roster", list, mapList.ToArray()),
				new ParameterRenderer("Audit_SelfCompanyName", "科/室"),
				new ParameterRenderer("Audit_HeadCompanyName", "部")
			));
		}
	}
}