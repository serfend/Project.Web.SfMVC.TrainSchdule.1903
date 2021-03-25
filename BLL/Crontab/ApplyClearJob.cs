using BLL.Interfaces;
using BLL.Interfaces.ApplyInfo;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Crontab
{
	public class ApplyClearJob
	{
		private IApplyVacationService applyService;
		private readonly IUserActionServices userActionServices;
        private readonly IApplyServiceClear applyServiceClear;

        public ApplyClearJob(IApplyVacationService applyService, IUserActionServices userActionServices,IApplyServiceClear applyServiceClear)
		{
			this.applyService = applyService;
			this.userActionServices = userActionServices;
            this.applyServiceClear = applyServiceClear;
        }

		/// <summary>
		/// 清除失效的申请
		/// </summary>
		public void Run(string HandleBy = "Default")
		{
			var r =  applyService.RemoveAllUnSaveApply(TimeSpan.FromDays(1)).Result;
			var r2 = applyServiceClear.RemoveAllNoneFromUserApply(TimeSpan.FromDays(1)).Result;
			var r3 = applyService.RemoveAllRemovedUsersApply().Result;
			userActionServices.Log(UserOperation.ModifyApply, "#System#", $"{HandleBy} - 清理未保存项:{r},无用户使用项{r2},已移除的用户项:{r3}", true, ActionRank.Warning);
		}
	}
}