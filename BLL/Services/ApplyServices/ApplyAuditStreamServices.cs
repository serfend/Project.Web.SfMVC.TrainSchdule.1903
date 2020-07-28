using Abp.Linq.Expressions;
using BLL.Extensions;
using BLL.Extensions.ApplyExtensions;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services.ApplyServices
{
	public class ApplyAuditStreamServices : IApplyAuditStreamServices
	{
		private readonly ApplicationDbContext context;
		private readonly ICompanyManagerServices companyManagerServices;

		public ApplyAuditStreamServices(ApplicationDbContext context, ICompanyManagerServices companyManagerServices)
		{
			this.context = context;
			this.companyManagerServices = companyManagerServices;
		}

		public ApplyAuditStreamNodeAction EditNode(string nodeName, Func<ApplyAuditStreamNodeAction, bool> callback)
		{
			var node = context.ApplyAuditStreamNodeActions.Where(a => a.Name == nodeName).FirstOrDefault();
			if (callback != null && callback.Invoke(node)) context.SaveChanges();
			return node;
		}

		public ApplyAuditStream EditSolution(string solutionName, Func<ApplyAuditStream, bool> callback)
		{
			var solution = context.ApplyAuditStreams.Where(s => s.Name == solutionName).FirstOrDefault();
			if (callback != null && callback.Invoke(solution)) context.SaveChanges();
			return solution;
		}

		public ApplyAuditStreamSolutionRule EditSolutionRule(string solutionRuleName, Func<ApplyAuditStreamSolutionRule, bool> callback)
		{
			var rule = context.ApplyAuditStreamSolutionRules.Where(r => r.Name == solutionRuleName).FirstOrDefault();
			if (callback != null && callback.Invoke(rule)) context.SaveChanges();
			return rule;
		}

		public ApplyAuditStreamSolutionRule GetAuditSolutionRule(User user, bool CheckInvalidAccount)
		{
			if (user == null) return null;
			var cmp = user.CompanyInfo.Company.Code;
			// 寻找符合条件的方案，并按优先级排序后取第一个
			var auditRule = context.ApplyAuditStreamSolutionRules.Where(r => r.Enable).OrderByDescending(a => a.Priority).ToList();
			var fitRule = new List<ApplyAuditStreamSolutionRule>();
			foreach (var rule in auditRule)
			{
				if (GetToAuditMembers(cmp, rule, CheckInvalidAccount).Contains(user.Id))
					return rule;
			};
			return null;
		}

		public ApplyAuditStreamNodeAction NewNode(IMembersFilter filter, string name, string description = null)
		{
			var prev = context.ApplyAuditStreamNodeActions.Where(a => a.Name == name).FirstOrDefault();
			if (prev != null) return prev;
			var result = new ApplyAuditStreamNodeAction()
			{
				Description = description,
				Name = name,
				Create = DateTime.Now,

				Companies = filter?.Companies,
				CompanyRefer = filter?.CompanyRefer,
				CompanyTags = filter.CompanyTags,
				CompanyCodeLength = filter.CompanyCodeLength,
				Duties = filter?.Duties,
				DutiesTags = filter?.DutiesTags,
				DutyIsMajor = filter?.DutyIsMajor ?? DutiesIsMajor.BothCanGo,
				AuditMembers = filter?.AuditMembers,
				AuditMembersCount = filter?.AuditMembersCount ?? 0,
			};
			context.ApplyAuditStreamNodeActions.Add(result);
			context.SaveChanges();
			return result;
		}

		public ApplyAuditStream NewSolution(IEnumerable<ApplyAuditStreamNodeAction> Nodes, string name, string description = null)
		{
			var prev = context.ApplyAuditStreams.Where(a => a.Name == name).FirstOrDefault();
			if (prev != null) return prev;
			var result = new ApplyAuditStream()
			{
				Name = name,
				Description = description,
				Create = DateTime.Now,

				Nodes = string.Join("##", Nodes.Select(n => n.Name))
			};
			context.ApplyAuditStreams.Add(result);
			context.SaveChanges();
			return result;
		}

		public ApplyAuditStreamSolutionRule NewSolutionRule(ApplyAuditStream solution, IMembersFilter filter, string name, string description = null, int priority = 0, bool enable = false)
		{
			var prev = context.ApplyAuditStreamSolutionRules.Where(r => r.Name == name).FirstOrDefault();
			if (prev != null) return prev;
			var result = new ApplyAuditStreamSolutionRule()
			{
				Name = name,
				Description = description,
				Priority = priority,
				Enable = enable,
				Create = DateTime.Now,

				Companies = filter?.Companies,
				CompanyRefer = filter?.CompanyRefer,
				CompanyTags = filter.CompanyTags,
				CompanyCodeLength = filter.CompanyCodeLength,
				Duties = filter?.Duties,
				DutiesTags = filter?.DutiesTags,
				DutyIsMajor = filter?.DutyIsMajor ?? DutiesIsMajor.BothCanGo,
				AuditMembers = filter?.AuditMembers,
				AuditMembersCount = filter?.AuditMembersCount ?? 0,
				Solution = solution
			};
			context.ApplyAuditStreamSolutionRules.Add(result);
			context.SaveChanges();
			return result;
		}

		public IEnumerable<string> GetToAuditMembers(string company, IMembersFilter filterRaw, bool CheckInvalidAccount)
		{
			var filter = filterRaw.ToDtoModel();
			if (filter == null || company == null) return null;
			if (filter.AuditMembers != null && filter.AuditMembers.Any()) return filter.AuditMembers;
			var result = context.AppUsers.AsQueryable();

			// 指定单位
			string target = null;
			if (filter.CompanyRefer != null)
			{
				var refer = filter.CompanyRefer.ToLower();
				target = refer == "parent" ? company.Substring(0, company.Length - 1) : refer == "self" ? company : null;
			}
			if (target == null)
			{
				// Companies
				if (filter.Companies != null && filter.Companies.Any(a => a.Length > 0))
				{
					var expC = PredicateBuilder.New<User>(false);
					foreach (var c in filter.Companies)
						expC = expC.Or(u => EF.Functions.Like(u.CompanyInfo.Company.Code, $"{c}%"));
					result = result.Where(expC);
				}
				// CompanyTag
				if (filter.CompanyTags != null && filter.CompanyTags.Any(a => a.Length > 0))
				{
					var expC = PredicateBuilder.New<User>(false);
					foreach (var c in filter.CompanyTags)
						expC = expC.Or(u => EF.Functions.Like(u.CompanyInfo.Company.Tag, $"%{c}%"));
					result = result.Where(expC);
				}

				// CompanyLength
				if (filter.CompanyCodeLength != null && filter.CompanyCodeLength.Any(a => a > 0))
				{
					var expL = PredicateBuilder.New<User>(false);
					foreach (var l in filter.CompanyCodeLength)
						expL = expL.Or(u => u.CompanyInfo.Company.Code.Length == l);
					result = result.Where(expL);
				}
			}
			else
				result = result.Where(u => u.CompanyInfo.Company.Code == target);

			// 指定职务
			if (filter.Duties != null && filter.Duties.Any(a => a > 0))
			{
				var expD = PredicateBuilder.New<User>(false);
				foreach (var d in filter.Duties)
					expD = expD.Or(u => u.CompanyInfo.Duties.Code == d);
				result = result.Where(expD);
			}
			if (filter.DutyTags != null && filter.DutyTags.Any(a => a.Length > 0))
			{
				var expD = PredicateBuilder.New<User>(false);
				foreach (var d in filter.DutyTags)
					expD = expD.Or(u => EF.Functions.Like(u.CompanyInfo.Duties.Tags, $"%{d}%"));
				result = result.Where(expD);
			}

			switch (filter.DutyIsMajor)
			{
				case DutiesIsMajor.BothCanGo: break;
				case DutiesIsMajor.OnlyMajor:
					result = result.Where(u => u.CompanyInfo.Duties.IsMajorManager);
					break;

				case DutiesIsMajor.OnlyUnMajor:
					result = result.Where(u => !u.CompanyInfo.Duties.IsMajorManager);
					break;
			}
			var rawUser = CheckInvalidAccount ?
				result.ToList().Where(u => u.Application.InvalidAccount() == UserExtensions.AccountType.BeenAuth).Select(u => u.Id)
				: result.ToList().Select(u => u.Id);
			// 管理具有本单位审批权限
			// 但非必选项，此处不应直接加入可审列表，而应在操作审批时判断是否是管理
			// 一旦管理审批，此流程将直接通过
			//var managers = companyManagerServices.GetManagers(company).ToList();
			//if (managers != null)
			//{
			//	var m = managers.Select(u => u.User.Id).ToList();
			//	fitUsers = fitUsers.Union(m);
			//}
			return rawUser;
		}

		public ApplyAuditStreamNodeAction GetNode(Guid id)
		{
			return context.ApplyAuditStreamNodeActions.Where(i => i.Id == id).FirstOrDefault();
		}

		public ApplyAuditStreamSolutionRule GetRule(Guid id)
		{
			return context.ApplyAuditStreamSolutionRules.Where(i => i.Id == id).FirstOrDefault();
		}

		public ApplyAuditStream GetSolution(Guid id)
		{
			return context.ApplyAuditStreams.Where(i => i.Id == id).FirstOrDefault();
		}
	}
}