using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions.ApplyExtensions
{
	public static class MembersFilterExtensions
	{
		public static MembersFilter ToModel(this MembersFilterDto model)
		{
			if (model == null) return null;
			return new MembersFilter()
			{
				AuditMembers = string.Join("##", model.AuditMembers),
				AuditMembersCount = model.AuditMembersCount,
				Companies = string.Join("##", model.Companies),
				CompanyRefer = model.CompanyRefer,
				CompanyTags = string.Join("##", model.CompanyTags),
				CompanyCodeLength = string.Join("##", model.CompanyCodeLength),
				Duties = string.Join("##", model.Duties),
				DutyIsMajor = model.DutyIsMajor
			};
		}

		public static MembersFilterDto ToDtoModel(this MembersFilter model)
		{
			if (model == null) return null;
			return new MembersFilterDto()
			{
				AuditMembers = model.AuditMembers?.Length == 0 ? Array.Empty<string>() : model.AuditMembers?.Split("##"),
				AuditMembersCount = model.AuditMembersCount,
				Companies = model.Companies?.Length == 0 ? Array.Empty<string>() : model.Companies?.Split("##"),
				CompanyRefer = model.CompanyRefer,
				Duties = model.Duties?.Length == 0 ? Array.Empty<int>() : model.Duties?.Split("##").Select(d => Convert.ToInt32(d)),
				DutyIsMajor = model.DutyIsMajor
			};
		}
	}
}