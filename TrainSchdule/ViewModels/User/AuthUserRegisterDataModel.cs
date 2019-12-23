using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 通过认证
	/// </summary>
	public class AuthUserRegisterDataModel
	{
		/// <summary>
		/// 需通过认证的账号
		/// </summary>
		public string UserName { get; set; }
	}
}
