using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace TrainSchdule.TsWebSocket
{
	/// <summary>
	/// 单个连接
	/// </summary>
	public class WebSocketConnection
	{
		public WebSocketConnection(User user, WebSocket webSocket)
		{
			User = user;
			Socket = webSocket;
		}

		/// <summary>
		/// 登录的用户
		/// </summary>
		public User User { get; private set; }

		/// <summary>
		/// 连接
		/// </summary>
		public WebSocket Socket { get; private set; }
	}
}