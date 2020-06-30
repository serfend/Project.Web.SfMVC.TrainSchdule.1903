using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.Crontab;

namespace BLL.Crontab
{
	public class UserInfoClearJob : ICrontabJob
	{
		private readonly IUsersService userServices;

		public UserInfoClearJob(IUsersService userServices)
		{
			this.userServices = userServices;
		}

		/// <summary>
		/// 清除失效的用户信息
		/// </summary>
		public void Run()
		{
			Task.Run(userServices.RemoveNoRelateInfo).Wait();
		}
	}
}