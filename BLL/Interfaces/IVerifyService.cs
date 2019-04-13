using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
	public interface IVerifyService:IDisposable
	{
		/// <summary>
		/// 初始化验证码
		/// </summary>
		void Get();
		/// <summary>
		/// 验证码是否正确
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		bool Verify(int code);
	}
}
