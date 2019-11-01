using System;
using System.DrawingCore;

namespace BLL.Interfaces
{
	public interface IVerifyService
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
		string Verify(int code);

		string Status { get;  }
		Point Pos { get; }
	}
}
