using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.UserInfo;
using DAL.Entities.ZX.Phy;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions.Common
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

		public static ApiResult NotExist(this User model) => ActionStatusMessage.UserMessage.NotExist;

		public static ApiResult NotExist(this GradePhyRecord model) => ActionStatusMessage.Grade.Record.NotExist;
	}
}