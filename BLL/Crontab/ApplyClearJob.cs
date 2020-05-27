using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Crontab
{
	public class ApplyClearJob : ICrontabJob
	{
		private IApplyService applyService;

		public ApplyClearJob(IApplyService applyService)
		{
			this.applyService = applyService;
		}

		/// <summary>
		/// 清除失效的申请
		/// </summary>
		public void Run()
		{
			Task.Run(applyService.RemoveAllUnSaveApply).Wait();
			Task.Run(applyService.RemoveAllNoneFromUserApply).Wait();
		}
	}
}