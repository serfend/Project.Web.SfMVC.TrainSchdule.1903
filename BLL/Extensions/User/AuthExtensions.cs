using GoogleAuth;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions
{
	public static class AuthExtensions
	{
		/// <summary>
		/// 生成授权码二维码链接
		/// </summary>
		/// <param name="model"></param>
		/// <param name="issuer"></param>
		/// <returns></returns>
		public static string QrCodeUrl(this Auth model, string issuer)
		{
			bool isInvalid = model?.UserName == null || model?.Password == null;
			var issuerShow = isInvalid ? (issuer ?? "XT2U") : "无效的授权码";
			return $"otpauth://totp/{model?.UserName}?secret={model?.Password}&issuer={issuerShow}";
		}
	}
}