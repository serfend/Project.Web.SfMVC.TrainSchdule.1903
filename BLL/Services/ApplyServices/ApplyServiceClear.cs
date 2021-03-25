using BLL.Interfaces.ApplyInfo;
using DAL.Data;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ApplyServices
{
	public class ApplyServiceClear : IApplyServiceClear
	{
        private readonly ApplicationDbContext context;

        public ApplyServiceClear(ApplicationDbContext context)
        {
            this.context = context;
        }


		public async Task<int> RemoveAllNoneFromUserApply(TimeSpan interval)
		{
			var applies = context.Applies;
			var applyIndays = context.AppliesInday;
			var outofDate = DateTime.Now.Subtract(interval);

			#region request

			//寻找所有没有创建申请且不是今天创建的 请求信息
			var request = context.ApplyRequests.Where(r => r.CreateTime < outofDate).Where(a=> !applyIndays.Any(r => r.RequestInfo.Id == r.Id)).Where(r =>  !applies.Any(a => a.RequestInfo.Id == r.Id)).ToList();
			//删除这些请求信息的福利信息
			foreach (var r in request) context.VacationAdditionals.RemoveRange(r.AdditialVacations);
			//删除这些请求信息
			context.ApplyRequests.RemoveRange(request);

			#endregion request

			#region steps

			// 删除所有无申请指向的步骤
			var applySteps = context.ApplyAuditSteps.Where(s => !applies.Any(a => a.ApplyAllAuditStep.Any(step => step.Id == s.Id))).Where(s => !applyIndays.Any(a => a.ApplyAllAuditStep.Any(step => step.Id == s.Id)));
			context.ApplyAuditSteps.RemoveRange(applySteps);

			#endregion steps

			#region base

			//寻找所有没有创建申请且不是今天创建的 基础信息
			var baseInfos = context.ApplyBaseInfos.Where(r => DateTime.Now.Date != r.CreateTime.Date).Where(r => !applies.Any(a => a.BaseInfo.Id == r.Id)).Where(r => !applyIndays.Any(a => a.BaseInfo.Id == r.Id));
			//删除这些基础信息
			context.ApplyBaseInfos.RemoveRange(baseInfos);

			#endregion base

			#region response

			//寻找所有没有被引用了的反馈信息
			var responses = context.ApplyResponses.Where(r => !applies.Any(a => a.Response.Any(ar => ar.Id == r.Id))).Where(r => !applyIndays.Any(a => a.Response.Any(ar => ar.Id == r.Id)));
			context.ApplyResponses.RemoveRange(responses);

			#endregion response

			await context.SaveChangesAsync().ConfigureAwait(true); // 立即执行

			return request.Count + applySteps.Count() + baseInfos.Count() + responses.Count();
		}

	}
}
