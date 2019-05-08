using System;

namespace BLL.Helpers
{
	public class ActionStatusMessageException:Exception
	{
		public Status Status { get; set; }
		public ActionStatusMessageException(Status status,string message=null):base(message)
		{
			Status = status;
		}
	}
}
