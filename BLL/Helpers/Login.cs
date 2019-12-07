namespace BLL.Helpers
{
	public static partial class ActionStatusMessage
	{
		public static partial class Account
		{
			public static class Login
			{
				public static readonly ApiResult Default = new ApiResult(11000, "登录验证失败");
				public static readonly ApiResult ByUnknown = new ApiResult(11100, "存在异常导致登录失败");
				public static readonly ApiResult AuthFormat = new ApiResult(11200, "输入格式错误");
				public static readonly ApiResult AuthException = new ApiResult(11300, "异常登录");
				public static readonly ApiResult AuthBlock = new ApiResult(11400, "账号存在危险,已阻止");
				public static readonly ApiResult AuthAccountOrPsw = new ApiResult(11500, "账号或密码错误");
				public static readonly ApiResult PasswordIsSame = new ApiResult(11600, "新的密码与旧密码相同");
				
			}
		}
	}
}
