using BLL.Extensions.ApplyExtensions;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services.ApplyServices
{
	public class ApplyAuditStreamServices : IApplyAuditStreamServices
	{
		private readonly ApplicationDbContext context;

		public ApplyAuditStreamServices(ApplicationDbContext context)
		{
			this.context = context;
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

		public ApplyAuditStreamSolutionRule GetAuditSolutionRule(User user)
		{
			if (user == null) return null;
			var cmp = user.CompanyInfo.Company.Code;
			// 寻找符合条件的方案，并按优先级排序后取第一个
			var auditRule = context.ApplyAuditStreamSolutionRules.Where(r => r.Enable).ToList();
			var fitRule = new List<ApplyAuditStreamSolutionRule>();
			foreach (var rule in auditRule)
			{
				if (GetToAuditMembers(cmp, rule).Contains(user.Id))
					fitRule.Add(rule);
			};
			return fitRule.OrderByDescending(a => a.Priority).FirstOrDefault();
		}

		public ApplyAuditStreamNodeAction NewNode(MembersFilter filter, string name, string description = null)
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

		public ApplyAuditStreamSolutionRule NewSolutionRule(ApplyAuditStream solution, MembersFilter filter, string name, string description = null, int priority = 0, bool enable = false)
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

		public IEnumerable<string> GetToAuditMembers(string company, MembersFilter filterRaw)
		{
			var filter = filterRaw.ToDtoModel();
			if (filter == null || company == null) return null;
			if (filter.AuditMembers != null && filter.AuditMembers.Any(a => true)) return filter.AuditMembers;
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
				if (filter.Companies != null && filter.Companies.Any(a => true)) result = result.Where(u => filter.Companies.Any(c => c == u.CompanyInfo.Company.Code));

				// CompanyTag
				if (filter.CompanyTags != null && filter.CompanyTags.Any(a => true)) result = result.Where(u => filter.CompanyTags.Any(c => c == u.CompanyInfo.Company.Tag));

				// CompanyLength
				if (filter.CompanyCodeLength != null && filter.CompanyCodeLength.Any(a => true)) result = result.Where(u => filter.CompanyCodeLength.Any(c => c == u.CompanyInfo.Company.Code.Length));
			}
			else
				result = result.Where(u => u.CompanyInfo.Company.Code == target);

			// 指定职务
			if (filter.Duties == null || !filter.Duties.Any(a => true))
			{
				if (filter.DutyTags != null && filter.DutyTags.Any(a => true)) result = result.Where(u => filter.DutyTags.Any(t => u.CompanyInfo.Duties.Tags.Contains(t)));

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
			}
			else result = result.Where(u => filter.Duties.Any(d => d == u.CompanyInfo.Duties.Code));

			return result.Select(u => u.Id);
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