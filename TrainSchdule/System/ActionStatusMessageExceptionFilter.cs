using BLL.Helpers;
using BLL.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels;

namespace TrainSchdule.System
{
	/// <summary>
	/// 当发生业务逻辑错误自动回调
	/// </summary>
	public class ActionStatusMessageExceptionFilter : Attribute, IExceptionFilter
	{
		private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly IUserActionServices userActionServices;

        /// <summary>
        ///
        /// </summary>
        /// <param name="modelMetadataProvider"></param>
        /// <param name="userActionServices"></param>
        public ActionStatusMessageExceptionFilter(
			IModelMetadataProvider modelMetadataProvider,IUserActionServices userActionServices)
		{
			_modelMetadataProvider = modelMetadataProvider;
            this.userActionServices = userActionServices;
        }

		/// <summary>
		/// 发生异常进入
		/// </summary>
		/// <param name="context"></param>
		public void OnException(ExceptionContext context)
		{
			if (!context.ExceptionHandled)//如果异常没有处理
			{
				(context.Result,context.ExceptionHandled) = context.Exception switch
				{
					DbUpdateConcurrencyException => (new JsonResult(ActionStatusMessage.StaticMessage.System.SystemBusy),true),
					ModelStateException ex => (new JsonResult(ex.Model), true),
					ActionStatusMessageException ex => OnActionStatusException(context,ex),
					_ =>(null,false),
				};
			}
		}

		private (IActionResult,bool) OnActionStatusException(ExceptionContext context,ActionStatusMessageException ex)
        {
			userActionServices.Log(DAL.Entities.UserInfo.UserOperation.InvalidModel, null,$"{context.HttpContext.Request.Path}:{ex.Status.Message}");
			return (new JsonResult(ex.Status), true);

		}
	}
}