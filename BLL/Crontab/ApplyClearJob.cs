using BLL.Interfaces;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Crontab
{
	public class ApplyClearJob
	{
		private IApplyService applyService;
		private readonly IUserActionServices userActionServices;

		public ApplyClearJob(IApplyService applyService, IUserActionServices userActionServices)
		{
			this.applyService = applyService;
			this.userActionServices = userActionServices;
		}

		/// <summary>
		/// 清除失效的申请
		/// </summary>
		public void Run(string HandleBy = "Default")
		{
			Task.Run(async () =>
			{
				var r = await applyService.RemoveAllUnSaveApply(TimeSpan.FromDays(1)).ConfigureAwait(true);
				var r2 = await applyService.RemoveAllNoneFromUserApply(TimeSpan.FromDays(1)).ConfigureAwait(false);
				userActionServices.Log(UserOperation.ModifyApply, "#System#", $"{HandleBy} - 清理未保存项:{r},无用户项{r2}", true, ActionRank.Warning);
			}).Wait();
		}
	}
}