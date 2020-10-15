using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace TsWebSocket.WebSockets
{
	/// <summary>
	/// ws中间件
	/// </summary>
	public class WebSocketManagerMiddleware
	{
		private readonly RequestDelegate _next;
		private WebSocketHandler _webSocketHandler { get; set; }
		private IUsersService usersService;

		/// <summary>
		/// ws中间件
		/// </summary>
		/// <param name="next"></param>
		/// <param name="webSocketHandler"></param>
		/// <param name="usersService"></param>
		public WebSocketManagerMiddleware(RequestDelegate next,
										  WebSocketHandler webSocketHandler, IUsersService usersService)
		{
			_next = next;
			_webSocketHandler = webSocketHandler;
			this.usersService = usersService;
		}

		/// <summary>
		/// 建立连接
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task Invoke(HttpContext context)
		{
			if (!context.WebSockets.IsWebSocketRequest)
				return;
			var userid = context.User?.Identity?.Name;
			if (userid == null) return;
			var user = usersService.GetById(userid);
			if (user == null) return;
			var socket = await context.WebSockets.AcceptWebSocketAsync();
			await _webSocketHandler.OnConnected(socket, user);

			await Receive(socket, async (result, buffer) =>
			{
				if (result.MessageType == WebSocketMessageType.Text)
				{
					await _webSocketHandler.ReceiveAsync(socket, result, buffer);
					return;
				}
				else if (result.MessageType == WebSocketMessageType.Close)
				{
					await _webSocketHandler.OnDisconnected(socket);
					return;
				}
			});

			//TODO - investigate the Kestrel exception thrown when this is the last middleware
			//await _next.Invoke(context);
		}

		private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
		{
			try
			{
				var buffer = new byte[1024 * 4];

				while (socket.State == WebSocketState.Open)
				{
					var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
														   cancellationToken: CancellationToken.None);

					handleMessage(result, buffer);
				}
			}
			catch (Exception ex)
			{
			}
		}
	}
}