using BLL.Interfaces;
using BLL.Interfaces.ApplyInfo;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Crontab
{
	public class ApplyIndayClearJob
	{
		private IApplyInDayService applyService;
		private readonly IUserActionServices userActionServices;

		public ApplyIndayClearJob(IApplyInDayService applyService, IUserActionServices userActionServices)
		{
			this.applyService = applyService;
			this.userActionServices = userActionServices;
		}

		/// <summary>
		/// 清除失效的申请
		/// </summary>
		public void Run(string HandleBy = "Default")
		{
			var r = applyService.RemoveAllUnSaveApply(TimeSpan.FromDays(1)).Result;
			var r3 = applyService.RemoveAllRemovedUsersApply().Result;
			userActionServices.Log(UserOperation.ModifyApply, "#System#", $"请假 - {HandleBy} - 清理未保存项:{r},已移除的用户项:{r3}", true, ActionRank.Warning);
		}
	}
}
