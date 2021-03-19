using Abp.Extensions;
using BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrainSchdule.TsWebSocket.Frame;

namespace TsWebSocket.WebSockets
{
	/// <summary>
	///
	/// </summary>
	public class MessageNotifyHandler : WebSocketHandler
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="webSocketConnectionManager"></param>
		public MessageNotifyHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="socket"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public override async Task ReceiveBinaryAsync(WebSocket socket, byte[] data)
		{
			var content = new
			{
				id = Guid.NewGuid()
			};
			var item = new WebSocketApiResult(null, JsonSerializer.Serialize(content)) { };
			await SendMessageAsync(base.WebSocketConnectionManager.GetId(socket), item);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="socket"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public override async Task ReceiveTextAsync(WebSocket socket, string data)
		{
			var item = JsonSerializer.Deserialize<WebSocketApiResult>(data);
			if (item == null) item = new WebSocketApiResult(null, null) { Status = -1, Message = "无效的信息" };
			else if (item.Path.IsNullOrEmpty())
			{
				item.Path = null;
				item.Message = ActionStatusMessage.WebsocketMessage.Sys.InvalidPath.Message;
				item.Status = ActionStatusMessage.WebsocketMessage.Sys.InvalidPath.Status;
			}
			else
			{
				item.Message = ActionStatusMessage.WebsocketMessage.Sys.InvalidPath.Message;
				item.Status = ActionStatusMessage.WebsocketMessage.Sys.InvalidPath.Status;
			}
			await SendMessageAsync(base.WebSocketConnectionManager.GetId(socket), item);
		}
	}
}