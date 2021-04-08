using Abp.Extensions;
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

namespace BLL.Services.Audit
{
	public class ApplyAuditStreamRepositoryServices : IApplyAuditStreamServices
	{
		private readonly ApplicationDbContext context;
		private readonly ICompanyManagerServices companyManagerServices;

		public ApplyAuditStreamRepositoryServices(ApplicationDbContext context, ICompanyManagerServices companyManagerServices)
		{
			this.context = context;
			this.companyManagerServices = companyManagerServices;
		}

		public ApplyAuditStreamNodeAction EditNode(string nodeName, string entityType = null, Func<ApplyAuditStreamNodeAction, bool> callback=null)
		{
			var node = context.ApplyAuditStreamNodeActionDb.Where(a => a.Name == nodeName).Where(a=>a.EntityType == entityType).FirstOrDefault();
			if (callback != null && callback.Invoke(node)) context.SaveChanges();
			return node;
		}

		public ApplyAuditStream EditSolution(string solutionName, string entityType = null, Func<ApplyAuditStream, bool> callback=null)
		{
			var solution = context.ApplyAuditStreamsDb.Where(s => s.Name == solutionName).Where(a => a.EntityType == entityType).FirstOrDefault();
			if (callback != null && callback.Invoke(solution)) context.SaveChanges();
			return solution;
		}

		public ApplyAuditStreamSolutionRule EditSolutionRule(string solutionRuleName, string entityType = null, Func<ApplyAuditStreamSolutionRule, bool> callback=null)
		{
			var rule = context.ApplyAuditStreamSolutionRuleDb.Where(r => r.Name == solutionRuleName).Where(a => a.EntityType == entityType).FirstOrDefault();
			if (callback != null && callback.Invoke(rule)) context.SaveChanges();
			return rule;
		}

		public ApplyAuditStreamSolutionRule GetAuditSolutionRule(User user,string entityType, bool CheckInvalidAccount)
		{
			if (user == null) return null;
			var cmp = user.CompanyInfo.CompanyCode;

			// 寻找符合条件的方案，并按优先级排序后取第一个
			var fitRule = new List<ApplyAuditStreamSolutionRule>();
			var auditRule = context.ApplyAuditStreamSolutionRuleDb
				.Where(r => r.Enable)
				.Where(r => cmp.StartsWith(r.RegionOnCompany));
			List<string> list = entityType.Split('|').ToList();
			list.Add(null); // 添加一个通用匹配
			// 寻找第一个存在的规则设置
			foreach (var i in list)
			{
				var tmp_rule = auditRule
					.Where(r => r.EntityType == i);
                if (tmp_rule.Any())
                {
					fitRule = tmp_rule.OrderByDescending(a => a.Priority).ToList();
					break;
                }
			}
			foreach (var rule in fitRule)
			{
				if (GetToAuditMembers(cmp, rule.RegionOnCompany, rule, CheckInvalidAccount).Contains(user.Id))
					return rule;
			};
			return null;
		}

		public ApplyAuditStreamNodeAction NewNode(IMembersFilter filter, string name, string companyRegion, string description = null,string entityType=null)
		{
			var prev = context.ApplyAuditStreamNodeActionDb.Where(a => a.Name == name).Where(a=>a.EntityType== entityType).FirstOrDefault();
			if (prev != null) return prev;
			var result = new ApplyAuditStreamNodeAction()
			{
				Description = description,
				Name = name,
				Create = DateTime.Now,
				RegionOnCompany = companyRegion,
				EntityType= entityType,


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

		public ApplyAuditStream NewSolution(IEnumerable<ApplyAuditStreamNodeAction> Nodes, string name, string companyRegion, string description = null,string entityType =null)
		{
			var prev = context.ApplyAuditStreamsDb.Where(a => a.Name == name).Where(a=>a.EntityType== entityType).FirstOrDefault();
			if (prev != null) return prev;
			var result = new ApplyAuditStream()
			{
				Name = name,
				RegionOnCompany = companyRegion,
				Description = description,
				Create = DateTime.Now,
				EntityType= entityType,

				Nodes = string.Join("##", Nodes.Select(n => n.Name))
			};
			context.ApplyAuditStreams.Add(result);
			context.SaveChanges();
			return result;
		}

		public ApplyAuditStreamSolutionRule NewSolutionRule(ApplyAuditStream solution, IMembersFilter filter, string name, string companyRegion, string description = null, int priority = 0, bool enable = false,string entityType=null)
		{
			var prev = context.ApplyAuditStreamSolutionRuleDb.Where(r => r.Name == name).FirstOrDefault();
			if (prev != null) return prev;
			var result = new ApplyAuditStreamSolutionRule()
			{
				Name = name,
				Description = description,
				Priority = priority,
				Enable = enable,
				EntityType = entityType,
				Create = DateTime.Now,
				RegionOnCompany = companyRegion,

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

		public IEnumerable<string> GetToAuditMembers(string company, string companyRegion, IMembersFilter filterRaw, bool CheckInvalidAccount)
		{
			var filter = filterRaw.ToDtoModel();
			if (filter == null || company == null) return null;
			if (filter.AuditMembers != null && filter.AuditMembers.Any()) return filter.AuditMembers;
			// 下级作用域节点应可作用到上级，否则将可能导致无法选中成员
			var result = context.AppUsersDb;
			string target = null;
			if (!filter.CompanyRefer.IsNullOrEmpty())
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
                        expC = expC.Or(u => u.CompanyInfo.CompanyCode.StartsWith(c));
					result = result.Where(expC);
				}
				// CompanyTag
				if (filter.CompanyTags != null && filter.CompanyTags.Any(a => a.Length > 0))
				{
					var expC = PredicateBuilder.New<User>(false);
					foreach (var c in filter.CompanyTags)
                        expC = expC.Or(u => u.CompanyInfo.Company.Tag.Contains(c));
					result = result.Where(expC);
				}

				// CompanyLength
				if (filter.CompanyCodeLength != null && filter.CompanyCodeLength.Any(a => a > 0))
				{
					var expL = PredicateBuilder.New<User>(false);
					foreach (var l in filter.CompanyCodeLength)
						expL = expL.Or(u => u.CompanyInfo.CompanyCode.Length == l);
					result = result.Where(expL);
				}
			}
			else
				result = result.Where(u => u.CompanyInfo.CompanyCode == target);

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
					expD = expD.Or(u => u.CompanyInfo.Duties.Tags.Contains(d));
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
			//	var m = managers.Select(u => u.UserId).ToList();
			//	fitUsers = fitUsers.Union(m);
			//}
			return rawUser;
		}

		public ApplyAuditStreamNodeAction GetNode(Guid id)
			=> context.ApplyAuditStreamNodeActionDb.FirstOrDefault(i => i.Id == id);

		public ApplyAuditStreamSolutionRule GetRule(Guid id)
			=> context.ApplyAuditStreamSolutionRuleDb.FirstOrDefault(i => i.Id == id);

		public ApplyAuditStream GetSolution(Guid id)
			=> context.ApplyAuditStreamsDb.FirstOrDefault(i => i.Id == id);
	}
}