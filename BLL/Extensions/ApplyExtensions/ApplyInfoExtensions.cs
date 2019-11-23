using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions.ApplyExtensions
{
	public static class ApplyInfoExtensions
	{
		public static ApplyDetailDto ToDetaiDto(this Apply model,bool AuditAvailable)
		{
			var b = new ApplyDetailDto()
			{
				Base = model?.BaseInfo.From.ToSummaryDto(),
				Company = model.BaseInfo.Company,
				Create = model.Create,
				Duties = model.BaseInfo.Duties,
				Hidden = model.Hidden,
				RequestInfo = model.RequestInfo,
				Response = model.Response.Select(r => r.ToResponseDto()),
				Id = model.Id,
				Social = model.BaseInfo.Social,
				Status = model.Status,
				AuditLeader = model.AuditLeader,
				AuditAvailable = AuditAvailable,
				RecallId =model.RecallId
			};
			return b;
		}
		public static ApplySummaryDto ToSummaryDto(this Apply model, bool AuditAvailable)
		{

			var b = new ApplySummaryDto()
			{
				Create = model?.Create,
				Status = model.Status,
				NowAuditCompany = model.Response.FirstOrDefault(r => r.Status == Auditing.Received || r.Status == Auditing.Denied)?.Company.Name,
				Base = model.BaseInfo.ToDto(),
				UserBase = model.BaseInfo.From.ToSummaryDto(),
				Id = model.Id,
				Request = model.RequestInfo,
				AuditAvailable = AuditAvailable,
				RecallId=model.RecallId
			};
			return b;
		}

	}
}
