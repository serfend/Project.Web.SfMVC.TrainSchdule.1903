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
		public static TResult ToModel<TResult>(this MembersFilterDto model) where TResult : IMembersFilter, new()
		{
			if (model == null) return default;
			return new TResult()
			{
				AuditMembers = string.Join("##", model.AuditMembers),
				AuditMembersCount = model.AuditMembersCount,
				Companies = string.Join("##", model.Companies),
				CompanyRefer = model.CompanyRefer,
				CompanyTags = string.Join("##", model.CompanyTags),
				CompanyCodeLength = string.Join("##", model.CompanyCodeLength),
				Duties = string.Join("##", model.Duties),
				DutiesTags = string.Join("##", model.DutyTags),
				DutyIsMajor = model.DutyIsMajor,
				RegionOnCompany = model.CompanyRegion,
			};
		}

		public static MembersFilterDto ToDtoModel(this IMembersFilter model)
		{
			if (model == null) return null;
			return new MembersFilterDto()
			{
				AuditMembers = model.AuditMembers?.Length == 0 ? Array.Empty<string>() : model.AuditMembers?.Split("##"),
				AuditMembersCount = model.AuditMembersCount,
				Companies = model.Companies?.Length == 0 ? Array.Empty<string>() : model.Companies?.Split("##"),
				CompanyRefer = model.CompanyRefer,
				CompanyTags = model.CompanyTags?.Length == 0 ? Array.Empty<string>() : model.CompanyTags.Split("##"),
				CompanyCodeLength = model.CompanyCodeLength?.Length == 0 ? Array.Empty<int>() : model.CompanyCodeLength.Split("##").Select(d => Convert.ToInt32(d)),
				Duties = model.Duties?.Length == 0 ? Array.Empty<int>() : model.Duties?.Split("##").Select(d => Convert.ToInt32(d)),
				DutyTags = model.DutiesTags?.Length == 0 ? Array.Empty<string>() : model.DutiesTags?.Split("##"),
				DutyIsMajor = model.DutyIsMajor
			};
		}
	}
}