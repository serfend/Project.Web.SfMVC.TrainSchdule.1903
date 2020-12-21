using BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.TsWebSocket.Frame
{
	/// <summary>
	/// ws消息结果回调
	/// </summary>
	public class WebSocketApiResult : ApiResult
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="content"></param>
		public WebSocketApiResult(string path, string content)
		{
			Path = path;
			Content = content;
		}

		/// <summary>
		/// 标题
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// 内容
		/// </summary>
		public string Content { get; set; }
	}
}