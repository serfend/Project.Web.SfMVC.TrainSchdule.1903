using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions.ApplyExtensions
{
	public static class ApplyResponseExtensions
	{
		public static ApplyResponseDto SelfRankAuditStatus(this IEnumerable<ApplyResponseDto> model)
		{
			var list = model.ToList();
			if (list?.Count == 0) return null;
			var item = list.ElementAtOrDefault(0);
			return item;
		}

		public static ApplyResponseDto LastRankAuditStatus(this IEnumerable<ApplyResponseDto> model)
		{
			var list = model.ToList();

			if (list?.Count < 2) return null;
			var item = list.ElementAtOrDefault(list.Count - 2);
			return item;
		}

		public static string AuditResult(this ApplyResponseDto model)
		{
			if (model == null) return "无审批结果";
			var remark = (model.Remark != null && model.Remark.Length > 0) ? $"({model.Remark})" : null;
			return $"{model.Status.ToStr()}{remark}";
		}

		public static ApplyResponseDto ToResponseDto(this ApplyResponse model)
		{
			var b = new ApplyResponseDto()
			{
				AuditingUserRealName = model?.AuditingBy?.BaseInfo?.RealName,
				AuditingUserId = model?.AuditingBy?.Id,
				Index = model.StepIndex,
				HandleStamp = model.HandleStamp,
				Remark = model.Remark,
				Status = model.Status
			};
			return b;
		}
	}
}