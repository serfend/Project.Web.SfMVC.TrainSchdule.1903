using DAL.Entities.Common;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions.CreateClientInfo
{
	public static class ClientInfoExtensions
	{
		public static TResult ClientInfo<TResult>(this HttpContext context) where TResult : ICreateClientInfo, new()
		{
			if (context == null) return default;
			return new TResult()
			{
				Device = context.Request.Headers["Device"],
				UA = context.Request.Headers["User-Agent"],
				Ip = context.Connection.RemoteIpAddress.ToString(),
			};
		}
	}
}