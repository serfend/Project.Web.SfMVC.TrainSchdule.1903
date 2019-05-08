using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TrainSchdule.Extensions
{
	/// <summary>
	/// 
	/// </summary>
	public static class ModelStateExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="modelState"></param>
		/// <returns></returns>
		public static IEnumerable<ShowError> AllModelStateErrors(this ModelStateDictionary modelState)
		{
			var result = new List<ShowError>();
			//找到出错的字段以及出错信息
			var errorFieldsAndMsgs = modelState.Where(m => m.Value.Errors.Any())
				.Select(x => new { x.Key, x.Value.Errors });
			foreach (var item in errorFieldsAndMsgs)
			{
				//获取键
				var fieldKey = item.Key.IsNullOrEmpty()?"Unknown":item.Key;
				//获取键对应的错误信息
				var fieldErrors = item.Errors
					.Select(e => new ShowError(fieldKey, e.ErrorMessage.IsNullOrEmpty()?e.Exception.Message:e.ErrorMessage));
				result.AddRange(fieldErrors);
			}
			return result;
		}
	}
	/// <summary>
	/// 
	/// </summary>
	public class ShowError
	{
		private string _message;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="message"></param>
		public ShowError(string key, string message)
		{
			Key = key;
			Message = message;
		}
		/// <summary>
		/// 
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Message
		{
			get => _message;
			set => _message = value;
		}
	}
}
