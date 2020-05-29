using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Extensions.Common
{
	/// <summary>
	///
	/// </summary>
	public static class EntityExtensions
	{
		/// <summary>
		/// 资源不存在
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static ApiResult NotExist(this BaseEntity model) => ActionStatusMessage.Static.ResourceNotExist;

		/// <summary>
		/// 用户不存在
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static ApiResult NotExist(this User model) => ActionStatusMessage.UserMessage.NotExist;
	}
}