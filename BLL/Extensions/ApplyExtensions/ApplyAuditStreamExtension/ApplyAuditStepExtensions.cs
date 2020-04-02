using DAL.DTO.Apply.ApplyAuditStreamDTO;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension
{
	public static class ApplyAuditStepExtensions
	{
		public static ApplyAuditStepDto ToDtoModel(this ApplyAuditStep model)
		{
			if (model == null) return null;
			return new ApplyAuditStepDto()
			{
				Id = model.Id,
				MembersAcceptToAudit = model.MembersAcceptToAudit?.Length == 0 ? Array.Empty<string>() : model.MembersAcceptToAudit.Split("##"),
				MembersFitToAudit = model.MembersFitToAudit?.Length == 0 ? Array.Empty<string>() : model.MembersFitToAudit.Split("##"),
				RequireMembersAcceptCount = model.RequireMembersAcceptCount,
				Index = model.Index
			};
		}
	}
}