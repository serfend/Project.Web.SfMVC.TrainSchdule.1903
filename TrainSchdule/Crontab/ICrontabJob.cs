using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Crontab
{
	/// <summary>
	/// 定时工作任务
	/// </summary>
	public interface ICrontabJob
	{
		/// <summary>
		/// 执行任务
		/// </summary>
		 void Run();
	}
}
