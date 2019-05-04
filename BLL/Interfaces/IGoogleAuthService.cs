using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;

namespace BLL.Interfaces
{
	/// <summary>
	/// 实现谷歌验证
	/// </summary>
	public interface IGoogleAuthService : IDisposable
	{
		bool Verify(int code,[Optional]string id, [Optional]string password);
		int Code([Optional] string id, [Optional] string password);
		void InitCode([Optional] string id, [Optional] string password);
		string GetAuthKey([Optional] string password);
		string Issuer { get; set; }
		string Url { get; }

		ICurrentUserService CurrentUserService { get; set; }
	}
}
