using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	public interface IVerifyService:IDisposable
	{
		/// <summary>
		/// 初始化验证码
		/// </summary>
		Guid Generate();
		/// <summary>
		/// 获取前景拖动验证码
		/// </summary>
		/// <returns></returns>
		byte[] Front();
		/// <summary>
		/// 获取背景验证码
		/// </summary>
		byte[] Background();
		/// <summary>
		/// 验证码是否正确
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		bool Verify(int code);

		string Status { get;  }
	}
}
