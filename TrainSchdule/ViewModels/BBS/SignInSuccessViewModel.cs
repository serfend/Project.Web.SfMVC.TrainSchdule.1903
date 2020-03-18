using BLL.Helpers;
using DAL.Entities.BBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.BBS
{
	public class SignInSuccessViewModel : ApiResult
	{
		public SignInSuccessDataModel Data { get; set; }
	}

	public class SignInSuccessDataModel
	{
		/// <summary>
		/// 签到成功描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 签到
		/// </summary>
		public SignIn SignIn { get; set; }
	}
}