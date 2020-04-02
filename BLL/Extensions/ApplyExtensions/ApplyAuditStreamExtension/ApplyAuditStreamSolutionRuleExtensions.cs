using DAL.DTO.Apply.ApplyAuditStreamDTO;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension
{
	public static class ApplyAuditStreamSolutionRuleExtensions
	{
		public static ApplyAuditStreamSolutionRuleDto ToSolutionRuleDtoModel(this ApplyAuditStreamSolutionRule model)
		{
			if (model == null) return null;
			return new ApplyAuditStreamSolutionRuleDto()
			{
				AuditMembers = model.AuditMembers?.Length == 0 ? Array.Empty<string>() : model.AuditMembers?.Split("##"),
				AuditMembersCount = model.AuditMembersCount,
				Companies = model.Companies?.Length == 0 ? Array.Empty<string>() : model.Companies?.Split("##"),
				CompanyRefer = model.CompanyRefer,
				Create = model.Create,
				Description = model.Description,
				Duties = model.Duties?.Length == 0 ? Array.Empty<int>() : model.Duties?.Split("##").Select(d => Convert.ToInt32(d)),
				DutyIsMajor = model.DutyIsMajor,
				Enable = model.Enable,
				Name = model.Name,
				Priority = model.Priority,
				SolutionName = model.SolutionName
			};
		}
	}
}