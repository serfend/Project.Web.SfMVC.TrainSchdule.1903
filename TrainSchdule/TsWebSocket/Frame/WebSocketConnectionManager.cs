using DAL.Entities.UserInfo;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using TrainSchdule.TsWebSocket;

namespace TsWebSocket.WebSockets
{
	/// <summary>
	/// ws管理
	/// </summary>
	public class WebSocketConnectionManager
	{
		private ConcurrentDictionary<string, WebSocketConnection> _sockets = new ConcurrentDictionary<string, WebSocketConnection>();

		/// <summary>
		/// 通过id获取链接
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public WebSocketConnection GetSocketById(string id) => _sockets.ContainsKey(id) ? _sockets[id] : null;
		

		/// <summary>
		/// 获取所有连接
		/// </summary>
		/// <returns></returns>
		public ConcurrentDictionary<string, WebSocketConnection> GetAll() => _sockets;

		/// <summary>
		/// 通过socket获取id
		/// </summary>
		/// <param name="socket"></param>
		/// <returns></returns>
		public string GetId(WebSocket socket) => _sockets.FirstOrDefault(p => p.Value?.Socket == socket).Key;

		/// <summary>
		/// 添加新的连接
		/// </summary>
		/// <param name="socket"></param>
		/// <param name="user"></param>
		public void AddSocket(WebSocket socket, User user)
		{
			var ws = GetSocketById(user.Id);
			if (ws != null) Task.Run(() => RemoveSocket(user.Id));
			_sockets.TryAdd(user.Id, new WebSocketConnection(user, socket));
		}

		/// <summary>
		/// 移除连接
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task RemoveSocket(string id)
		{
			try
			{
				WebSocketConnection socket;

				_sockets.TryRemove(id, out socket);

				await socket.Socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
			}
			catch (Exception)
			{
			}
		}
	}
}