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
					public static readonly ApiResult CipperInvalid = new ApiResult(12130, "无效的加密验证信息");
				}

				public static class Verify
				{
					public static readonly ApiResult Default = new ApiResult(12200, "验证码异常");
					public static readonly ApiResult Invalid = new ApiResult(12210, "验证码错误");
					public static readonly ApiResult NotSet = new ApiResult(12220, "验证码未输入");
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
					public static readonly ApiResult SystemAllReadyValid = new ApiResult(12430, "用户的注册申请已被审批");
					public static readonly ApiResult SystemInvalid = new ApiResult(12440, "用户未被授权使用系统");
					public static readonly ApiResult SystemAllReadyInvalid = new ApiResult(12450, "用户的注册申请已被退回，需重新注册");
					public static readonly ApiResult AuthUserNotExist = new ApiResult(12460, "无效的授权人 - 不存在");
					public static readonly ApiResult AuthUserNotSet = new ApiResult(12470, "无效的授权人 - 未设置");
				}
			}

			public static class Register
			{
				public static readonly ApiResult Default = new ApiResult(13000, "未知的注册错误");
				public static readonly ApiResult UserExist = new ApiResult(13100, "用户已存在");
				public static readonly ApiResult ConfirmPasswordNotSame = new ApiResult(13200, "两次输入的密码不一致");
				public static readonly ApiResult CidExist = new ApiResult(13300, "身份证已被使用");
				public static readonly ApiResult RootCompanyRequireAdminRight = new ApiResult(13400, "注册为最高层级单位时需要管理员权限。如需选择子层级单位，请登录您同事的账号后刷新页面选取。");
			}
		}
	}
}