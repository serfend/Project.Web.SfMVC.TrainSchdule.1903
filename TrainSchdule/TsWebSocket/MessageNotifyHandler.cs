using BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

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
		/// <param name="result"></param>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
		{
			string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
			await SendMessageAsync(WebSocketConnectionManager.GetId(socket), System.Text.Json.JsonSerializer.Serialize(new ApiResult(ActionStatusMessage.StaticMessage.ResourceNotExist, message, true)));
		}
	}
}