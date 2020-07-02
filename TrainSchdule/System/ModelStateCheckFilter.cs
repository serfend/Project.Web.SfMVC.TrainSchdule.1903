using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels;

namespace TrainSchdule.System
{
	/// <summary>
	/// 检查视图
	/// </summary>
	public class ModelStateCheckFilter : ActionFilterAttribute
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="context"></param>
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
				context.Result = new JsonResult(context.ModelState.ToModel());
			}
		}
	}
}