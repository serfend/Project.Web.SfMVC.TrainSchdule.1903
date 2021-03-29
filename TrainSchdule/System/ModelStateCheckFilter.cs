using BLL.Helpers;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.ViewModels;

namespace TrainSchdule.System
{
	/// <summary>
	/// 检查视图
	/// </summary>
	public class ModelStateCheckFilter : ActionFilterAttribute
	{
        private readonly IUserActionServices userActionServices;

        /// <summary>
        /// 
        /// </summary>
        public ModelStateCheckFilter(IUserActionServices userActionServices)
        {
            this.userActionServices = userActionServices;
        }
		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				var model = context.ModelState.ToModel();
				var modelExcept = string.Join(";", model.Data.List.ToList().Select(o=>$"{o.Key}:{o.Message}"));
				var route = context.HttpContext.Request.Path.Value;
				userActionServices.Log(DAL.Entities.UserInfo.UserOperation.InvalidModel, context.HttpContext.User?.Identity?.Name, $"{route},{modelExcept}");
				context.Result = new JsonResult(model);
			}
		}
	}
}