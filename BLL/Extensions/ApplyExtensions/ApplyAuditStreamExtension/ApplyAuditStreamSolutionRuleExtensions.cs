using BLL.Interfaces;
using DAL.DTO.Apply.ApplyAuditStreamDTO;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DAL.DTO.Apply.ApplyAuditStreamDTO.ApplyAuditStreamSolutionRuleDto;

namespace BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension
{
	public static class ApplyAuditStreamSolutionRuleExtensions
	{
		public static ApplyAuditStreamSolutionRuleDto ToSolutionRuleDtoModel(this ApplyAuditStreamSolutionRule model)
		{
			if (model == null) return null;
			return new ApplyAuditStreamSolutionRuleDto()
			{
				AuditMembers = (model.AuditMembers?.Length ?? 0) == 0 ? Array.Empty<string>() : model.AuditMembers?.Split("##"),
				AuditMembersCount = model.AuditMembersCount,
				Companies = (model.Companies?.Length ?? 0) == 0 ? Array.Empty<string>() : model.Companies?.Split("##"),
				CompanyRefer = model.CompanyRefer,
				CompanyTags = (model.CompanyTags?.Length ?? 0) == 0 ? Array.Empty<string>() : model.CompanyTags.Split("##"),
				CompanyCodeLength = (model.CompanyCodeLength?.Length ?? 0) == 0 ? Array.Empty<int>() : model.CompanyCodeLength.Split("##").Select(d => Convert.ToInt32(d)),
				Create = model.Create,
				Description = model.Description,
				Duties = (model.Duties?.Length ?? 0) == 0 ? Array.Empty<int>() : model.Duties?.Split("##").Select(d => Convert.ToInt32(d)),
				DutyTags = (model.DutiesTags?.Length ?? 0) == 0 ? Array.Empty<string>() : model.DutiesTags?.Split("##"),
				DutyIsMajor = model.DutyIsMajor,
				Enable = model.Enable,
				Name = model.Name,
				Priority = model.Priority,
				SolutionName = model.Solution.Name
			};
		}

		public static ApplyAuditStreamSolutionRuleVDto ToSolutionRuleVDtoModel(this ApplyAuditStreamSolutionRuleDto model, IUsersService usersService, ICompaniesService companiesService)
		{
			if (model == null) return null;
			return new ApplyAuditStreamSolutionRuleVDto()
			{
				AuditMembers = model.AuditMembers?.Select(m => usersService.Get(m)?.ToSummaryDto()),
				AuditMembersCount = model.AuditMembersCount,
				Companies = model.Companies?.Select(c => companiesService.GetById(c)?.ToDto()),
				CompanyCodeLength = model.CompanyCodeLength,
				CompanyRefer = model.CompanyRefer,
				CompanyTags = model.CompanyTags,
				Create = model.Create,
				Description = model.Description,
				Duties = model.Duties?.Select(d => companiesService.GetDuties(d)?.ToDto()),
				DutyIsMajor = model.DutyIsMajor,
				DutyTags = model.DutyTags,
				Enable = model.Enable,
				Name = model.Name,
				Priority = model.Priority,
				SolutionName = model.SolutionName
			};
		}
	}
}