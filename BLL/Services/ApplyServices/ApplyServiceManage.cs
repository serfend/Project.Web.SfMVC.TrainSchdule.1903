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
			if (model.ExecuteStatus?.Value != null)
			{
				var success = int.TryParse(model.ExecuteStatus.Value, out var executeStatusInt);
				list = list.Where(a => (int)a.ExecuteStatus == executeStatusInt);
			}
			if (model.NowAuditBy != null) list = list.Where(a => EF.Functions.Like(a.NowAuditStep.MembersFitToAudit, $"%{model.NowAuditBy.Value }%"));
			if (model.AuditBy != null) list = list.Where(a => a.ApplyAllAuditStep.Any(s => EF.Functions.Like(s.MembersFitToAudit, $"%{model.AuditBy.Value}%")));
			if (model.UserStatus != null)
			{
				var status = model.UserStatus.Arrays.FirstOrDefault();
				list = list.Where(a => (a.BaseInfo.From.SocialInfo.Status & status) > 0);
			}
			if (model.CompanyStatus != null)
			{
				var status = model.CompanyStatus.Arrays.FirstOrDefault();
				list = list.Where(a => ((int)a.BaseInfo.Company.CompanyStatus & status) > 0);
			}
			if (model.CompanyType != null)
				list = list.Where(a => EF.Functions.Like(a.BaseInfo.Company.Tag, $"%{model.CompanyType.Value}%"));
			if (model.DutiesType != null)
				list = list.Where(a => EF.Functions.Like(a.BaseInfo.Duties.Tags, $"%{model.DutiesType.Value}%"));
			if (model.CreateCompany != null)
			{
				var arr = model.CreateCompany?.Arrays?.Select(i => $"{i}%");
				var exp = PredicateBuilder.New<Apply>(false);
				foreach (var item in arr)
					exp = exp.Or(p => EF.Functions.Like(p.BaseInfo.Company.Code, item));
				if (arr != null)
					list = list.Where(a => a.BaseInfo != null)
						.Where(a => a.BaseInfo.From != null)
						.Where(a => a.BaseInfo.Company != null)
						.Where(exp);
			}

			bool anyDateFilterIsLessThan30Days = false;
			if (model.Create != null)
			{
				list = list.Where(a => (a.Create >= model.Create.Start && a.Create <= model.Create.End));
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
			if (model.StampLeave?.Start != null) list = list.Where(a => (a.RequestInfo.StampLeave >= model.StampLeave.Start && a.RequestInfo.StampLeave <= model.StampLeave.End));
			anyDateFilterIsLessThan30Days |= (model.StampLeave == null || model.StampLeave.End.Subtract(model.StampLeave.Start).Days <= 360);

			if (model.StampReturn != null)
			{
				list = list.Where(a => (a.RequestInfo.StampReturn >= model.StampReturn.Start && a.RequestInfo.StampReturn <= model.StampReturn.End));
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
			var result = list.SplitPage(model.Pages);
			totalCount = result.Item2;
			return result.Item1;
		}

		public async Task<int> RemoveAllUnSaveApply(TimeSpan interval)
		{
			var outofDate = DateTime.Now.Subtract(interval);
			//寻找所有找过1天未保存的申请
			var list = _context.AppliesDb
						 .Where(a => a.Status == AuditStatus.NotSave)
						 .Where(a => a.Create.HasValue && a.Create.Value < outofDate).ToList();
			await RemoveApplies(list).ConfigureAwait(true);
			return list.Count;
		}

		public async Task<int> RemoveAllRemovedUsersApply()
		{
			var applies = _context.Applies;
			var to_remove = applies.Where(a =>
				 ((int)a.BaseInfo.From.AccountStatus & (int)AccountStatus.Abolish) > 0 ||
				 ((int)a.BaseInfo.From.AccountStatus & (int)AccountStatus.DisableVacation) > 0 ||
				 ((int)a.BaseInfo.From.AccountStatus & (int)AccountStatus.PrivateAccount) > 0 ||
				 a.BaseInfo.From.CompanyInfo.Title.DisableVacation
			);
			await RemoveApplies(to_remove).ConfigureAwait(false);
			return to_remove.Count();
		}

		public async Task<int> RemoveAllNoneFromUserApply(TimeSpan interval)
		{
			var applies = _context.Applies;
			var outofDate = DateTime.Now.Subtract(interval);

			#region request

			//寻找所有没有创建申请且不是今天创建的 请求信息
			var request = _context.ApplyRequests.Where(r => r.CreateTime < outofDate).Where(r => !applies.Any(a => a.RequestInfo.Id == r.Id)).ToList();
			//删除这些请求信息的福利信息
			foreach (var r in request) _context.VacationAdditionals.RemoveRange(r.AdditialVacations);
			//删除这些请求信息
			_context.ApplyRequests.RemoveRange(request);

			#endregion request

			#region steps

			// 删除所有无申请指向的步骤
			var applySteps = _context.ApplyAuditSteps.Where(s => !applies.Any(a => a.ApplyAllAuditStep.Any(step => step.Id == s.Id)));
			_context.ApplyAuditSteps.RemoveRange(applySteps);

			#endregion steps

			#region base

			//寻找所有没有创建申请且不是今天创建的 基础信息
			var baseInfos = _context.ApplyBaseInfos.Where(r => DateTime.Now.Date != r.CreateTime.Date).Where(r => !applies.Any(a => a.BaseInfo.Id == r.Id));
			//删除这些基础信息
			_context.ApplyBaseInfos.RemoveRange(baseInfos);

			#endregion base

			#region response

			//寻找所有没有被引用了的反馈信息
			var responses = _context.ApplyResponses.Where(r => !applies.Any(a => a.Response.Any(ar => ar.Id == r.Id)));
			_context.ApplyResponses.RemoveRange(responses);

			#endregion response

			await _context.SaveChangesAsync().ConfigureAwait(true); // 立即执行

			var list = _context.AppliesDb.Where(a => a.BaseInfo.From == null);
			await RemoveApplies(list).ConfigureAwait(true);
			return request.Count + applySteps.Count() + baseInfos.Count() + responses.Count();
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