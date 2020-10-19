using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Helpers
{
	public static partial class ActionStatusMessage
	{
		public static class WebsocketMessage
		{
			public static class Sys
			{
				public static readonly ApiResult Default = new ApiResult(90100, "系统错误");
				public static readonly ApiResult InvalidPath = new ApiResult(90110, "无效的资源请求");
			}
		}
	}
}