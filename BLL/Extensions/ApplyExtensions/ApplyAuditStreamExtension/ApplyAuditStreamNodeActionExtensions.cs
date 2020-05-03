using BLL.Interfaces;
using DAL.DTO.Apply.ApplyAuditStreamDTO;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension
{
	public static class ApplyAuditStreamNodeActionExtensions
	{
		public static ApplyAuditStreamNodeAction ToApplyAuditStreamNodeAction(this IMembersFilter model, ApplyAuditStreamNodeAction raw = null)
		{
			if (model == null) return null;
			if (raw == null) raw = new ApplyAuditStreamNodeAction();

			raw.AuditMembers = model.AuditMembers;
			raw.AuditMembersCount = model.AuditMembersCount;
			raw.Companies = model.Companies;
			raw.CompanyCodeLength = model.CompanyCodeLength;
			raw.CompanyRefer = model.CompanyRefer;
			raw.CompanyTags = model.CompanyTags;
			raw.Duties = model.Duties;
			raw.DutiesTags = model.DutiesTags;
			raw.DutyIsMajor = model.DutyIsMajor;
			return raw;
		}

		public static ApplyAuditStreamNodeActionDto ToNodeDtoModel(this ApplyAuditStreamNodeAction model, ApplyAuditStreamNodeActionDto raw = null)
		{
			if (model == null) return null;
			if (raw == null) raw = new ApplyAuditStreamNodeActionDto();
			raw.Id = model.Id;
			raw.AuditMembers = (model.AuditMembers?.Length ?? 0) == 0 ? Array.Empty<string>() : model.AuditMembers?.Split("##");
			raw.AuditMembersCount = model.AuditMembersCount;
			raw.Companies = (model.Companies?.Length ?? 0) == 0 ? Array.Empty<string>() : model.Companies?.Split("##");
			raw.CompanyRefer = model.CompanyRefer;
			raw.CompanyTags = (model.CompanyTags?.Length ?? 0) == 0 ? Array.Empty<string>() : model.CompanyTags.Split("##");
			raw.CompanyCodeLength = (model.CompanyCodeLength?.Length ?? 0) == 0 ? Array.Empty<int>() : model.CompanyCodeLength.Split("##").Select(d => Convert.ToInt32(d));
			raw.Create = model.Create;
			raw.Description = model.Description;
			raw.Duties = (model.Duties?.Length ?? 0) == 0 ? Array.Empty<int>() : model.Duties?.Split("##").Select(d => Convert.ToInt32(d));
			raw.DutyTags = (model.DutiesTags?.Length ?? 0) == 0 ? Array.Empty<string>() : model.DutiesTags?.Split("##");
			raw.DutyIsMajor = model.DutyIsMajor;
			raw.Name = model.Name;
			return raw;
		}

		public static ApplyAuditStreamNodeActionVDto ToNodeVDtoModel(this ApplyAuditStreamNodeActionDto model, IUsersService userServices, ICompaniesService companiesService, ApplyAuditStreamNodeActionVDto raw = null)
		{
			if (model == null) return null;
			if (raw == null) raw = new ApplyAuditStreamNodeActionVDto();

			raw.Id = model.Id;
			raw.AuditMembers = model.AuditMembers?.Select(m => userServices.Get(m)?.ToSummaryDto());
			raw.AuditMembersCount = model.AuditMembersCount;
			raw.Companies = model.Companies?.Select(c => companiesService.GetById(c)?.ToDto());
			raw.CompanyCodeLength = model.CompanyCodeLength;
			raw.CompanyRefer = model.CompanyRefer;
			raw.CompanyTags = model.CompanyTags;
			raw.Create = model.Create;
			raw.Description = model.Description;
			raw.Duties = model.Duties?.Select(d => companiesService.GetDuties(d)?.ToDto());
			raw.DutyIsMajor = model.DutyIsMajor;
			raw.DutyTags = model.DutyTags;
			raw.Name = model.Name;
			return raw;
		}
	}
}