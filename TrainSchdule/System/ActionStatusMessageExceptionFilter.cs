﻿using BLL.Helpers;
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

		/// <summary>
		///
		/// </summary>
		/// <param name="modelMetadataProvider"></param>
		public ActionStatusMessageExceptionFilter(
			IModelMetadataProvider modelMetadataProvider)
		{
			_modelMetadataProvider = modelMetadataProvider;
		}

		/// <summary>
		/// 发生异常进入
		/// </summary>
		/// <param name="context"></param>
		public void OnException(ExceptionContext context)
		{
			if (!context.ExceptionHandled)//如果异常没有处理
			{
				if (context.Exception is ActionStatusMessageException ex)
				{
					context.Result = new JsonResult(ex.Status);
					context.ExceptionHandled = true;//异常已处理
				}
				else if (context.Exception is DbUpdateConcurrencyException ex2)
				{
					context.Result = new JsonResult(ActionStatusMessage.StaticMessage.System.SystemBusy);
					context.ExceptionHandled = true;
				}
			}
		}
	}
}