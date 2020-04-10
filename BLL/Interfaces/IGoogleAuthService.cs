using BLL.Services;
using GoogleAuth;
using System;
using System.Runtime.InteropServices;

namespace BLL.Interfaces
{
	/// <summary>
	/// 实现谷歌验证
	/// </summary>
	public interface IGoogleAuthService
	{
		AuthServiceModel Init(string username);

		/// <summary>
		/// 验证授权码正确性
		/// </summary>
		/// <param name="code"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		bool Verify(int code, string username);

		/// <summary>
		/// 获取当前授权码
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		int Code(string username);

		/// <summary>
		/// 获取指定用户的授权码
		/// </summary>
		/// <returns></returns>
		string GetAuthKey(string username);
	}
}