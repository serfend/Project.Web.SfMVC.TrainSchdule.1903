using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TrainSchdule.Extensions
{
	public static class ModelStateExtensions
	{
		public static IEnumerable<ShowError> AllModelStateErrors(this ModelStateDictionary modelState)
		{
			var result = new List<ShowError>();
			//找到出错的字段以及出错信息
			var errorFieldsAndMsgs = modelState.Where(m => m.Value.Errors.Any())
				.Select(x => new { x.Key, x.Value.Errors });
			foreach (var item in errorFieldsAndMsgs)
			{
				//获取键
				var fieldKey = item.Key;
				//获取键对应的错误信息
				var fieldErrors = item.Errors
					.Select(e => new ShowError(fieldKey, e.ErrorMessage));
				result.AddRange(fieldErrors);
			}
			return result;
		}
	}
	public class ShowError
	{
		public ShowError(string key, string message)
		{
			Key = key;
			Message = message;
		}
		public string Key { get; set; }
		public string Message { get; set; }
	}
}
