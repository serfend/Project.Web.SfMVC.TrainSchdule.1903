using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Common
{
	public interface ICreateClientInfo
	{
		/// <summary>
		/// 操作时使用的ip
		/// </summary>
		string Ip { get; set; }

		/// <summary>
		/// 通过接口类型判断用户所用的终端类型
		/// </summary>
		string Device { get; set; }

		/// <summary>
		/// 用户浏览器UserAgent
		/// </summary>
		string UA { get; set; }
	}

	public class BaseCreateClientInfo : ICreateClientInfo
	{
		public string Ip { get; set; }
		public string Device { get; set; }
		public string UA { get; set; }
	}
}