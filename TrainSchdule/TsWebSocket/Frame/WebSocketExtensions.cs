using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TsWebSocket.WebSockets
{
	/// <summary>
	/// ws 服务
	/// </summary>
	public static class WebSocketExtensions
	{
		/// <summary>
		/// 指定ws路径对应的服务
		/// </summary>
		/// <param name="app"></param>
		/// <param name="path"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
															  PathString path,
															  WebSocketHandler handler)
		{
			return app.Map(path, (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(handler));
		}

		/// <summary>
		/// 添加一个ws服务管理
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
		{
			services.AddSingleton<WebSocketConnectionManager>();

			foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
			{
				if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
				{
					services.AddSingleton(type);
				}
			}

			return services;
		}
	}
}