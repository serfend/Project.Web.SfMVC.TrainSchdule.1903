namespace BLL.Helpers
{
	public static partial class ActionStatusMessage
	{
		public static partial class Account
		{
			public static class Auth
			{
				public static class Invalid
				{
					public static readonly ApiResult Default = new ApiResult(12100, "访问被拒绝");
					public static readonly ApiResult Unknown = new ApiResult(12110, "存在不存在的操作");
					public static readonly ApiResult NotLogin = new ApiResult(12120, "用户未登录");
				}

				public static class Verify
				{
					public static readonly ApiResult Default = new ApiResult(12200, "验证码异常");
					public static readonly ApiResult Invalid = new ApiResult(12210, "验证码错误");
				}

				public static class AuthCode
				{
					public static readonly ApiResult Default = new ApiResult(12300, "授权码异常");
					public static readonly ApiResult Invalid = new ApiResult(12310, "授权码错误");
				}

				public static class Permission
				{
					public static readonly ApiResult Default = new ApiResult(12400, "授权异常");
					public static readonly ApiResult Exist = new ApiResult(12410, "用户已具有此权限");
					public static readonly ApiResult NotExist = new ApiResult(12420, "授权规则不存在");
				}
				
			}
			public static class Register { 
				public static readonly ApiResult Default = new ApiResult(13000, "用户已存在");
				public static readonly ApiResult UserExist = new ApiResult(13100, "用户已存在");
			}
		}
	}
}
