using System;

namespace BLL.Helpers
{
	public class ActionStatusMessageException : Exception 
	{
		public ApiResult Status { get; set; }

		public ActionStatusMessageException(ApiResult status, string message = null) : base(message)
		{
			Status = status;
		}
	}
}