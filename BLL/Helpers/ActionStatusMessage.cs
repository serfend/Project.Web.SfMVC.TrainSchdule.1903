using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSchdule.BLL.Helpers
{
	public static class ActionStatusMessage
	{
		public static readonly Status Success=new Status(0,"");
		public static readonly Status AccountLogin_InvalidAuth=new Status(600,"登录验证失败");
		public static readonly Status AccountLogin_InvalidByUnknown=new Status(60000,"存在异常导致登录失败");

		public static readonly Status AccountLogin_InvalidAuthFormat=new Status(60001,"输入格式错误");
		public static readonly Status AccountLogin_InvalidAuthException=new Status(60002,"异常登录");
		public static readonly Status AccountLogin_InvalidAuthBlock=new Status(60004,"账号存在危险,已阻止");
		public static readonly Status AccountLogin_InvalidAuthAccountOrPsw=new Status(60008,"账号或密码错误");
		public static readonly Status AccountAuth_Forbidden = new Status(403,"账号权限不足");
		public static readonly Status AccountAuth_Invalid = new Status(40301,"登录凭证已失效");

		public static readonly Status Company_NotExist = new Status(140001,"单位不存在");

	}

	public struct Status
	{
		public int Code;
		public string Message;

		public Status(int code, string message)
		{
			this.Code = code;
			this.Message = message;
		}
	}
}
