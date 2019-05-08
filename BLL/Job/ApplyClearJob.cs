using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interfaces;
using DAL.Data;
using Pomelo.AspNetCore.TimedJob;

namespace BLL.Job
{
	public class ApplyClearJob:Pomelo.AspNetCore.TimedJob.Job
	{
		private ApplicationDbContext _context;
		private IApplyService _applyService;

		public ApplyClearJob(ApplicationDbContext context, IApplyService applyService)
		{
			_context = context;
			_applyService = applyService;
		}

		// Begin 起始时间；Interval执行时间间隔，单位是毫秒，建议使用以下格式，此处为3小时；
		//SkipWhileExecuting是否等待上一个执行完成，true为等待；
		[Invoke(Begin = "2019-5-7 4:00", Interval = 1000 * 10, SkipWhileExecuting = true)]
		public void Run()
		{
			_applyService.RemoveAllUnSaveApply();
			
		}
	}
}
