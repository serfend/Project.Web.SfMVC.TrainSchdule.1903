using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrainSchdule.TsWebSocket;

namespace TsWebSocket.WebSockets
{
	/// <summary>
	///
	/// </summary>
	public abstract class WebSocketHandler
	{
		/// <summary>
		///
		/// </summary>
		public WebSocketConnectionManager WebSocketConnectionManager { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="webSocketConnectionManager"></param>
		public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager)
		{
			WebSocketConnectionManager = webSocketConnectionManager;
		}

		/// <summary>
		/// 建立连接
		/// </summary>
		/// <param name="socket"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public virtual async Task OnConnected(WebSocket socket, User user)
		{
			await Task.Run(() => WebSocketConnectionManager.AddSocket(socket, user));
		}

		/// <summary>
		/// 断开连接
		/// </summary>
		/// <param name="socket"></param>
		/// <returns></returns>
		public virtual async Task OnDisconnected(WebSocket socket)
		{
			await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket));
		}

		/// <summary>
		/// 发送信息给单个连接
		/// </summary>
		/// <param name="socket"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public async Task SendMessageAsync(WebSocketConnection socket, string message)
		{
			if (socket.Socket.State != WebSocketState.Open)
				return;

			await socket.Socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
																  offset: 0,
																  count: message.Length),
								   messageType: WebSocketMessageType.Text,
								   endOfMessage: true,
								   cancellationToken: CancellationToken.None);
		}

		/// <summary>
		/// 发送信息给单个用户
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public async Task SendMessageAsync(string userid, string message)
		{
			try
			{
				await SendMessageAsync(WebSocketConnectionManager.GetSocketById(userid), message);
			}
			catch (Exception)
			{
			}
		}

		/// <summary>
		/// 发送到所有在线的用户
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public async Task SendMessageToAllAsync(string message)
		{
			foreach (var pair in WebSocketConnectionManager.GetAll())
			{
				if (pair.Value.Socket.State == WebSocketState.Open)
					await SendMessageAsync(pair.Value, message);
			}
		}

		/// <summary>
		/// 接收新信息回调
		/// </summary>
		/// <param name="socket"></param>
		/// <param name="result"></param>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
	}
}