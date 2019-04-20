using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSchdule.BLL.Helpers
{
	public static class ActionStatusMessage
	{
		public static readonly Status Success = new Status(0, null);
		public static readonly Status Fail = new Status(-1, "失败");
		public static class Account
		{
			public static class Login
			{
				public static readonly Status Default = new Status(11000, "登录验证失败");
				public static readonly Status ByUnknown = new Status(11100, "存在异常导致登录失败");
				public static readonly Status AuthFormat = new Status(11200, "输入格式错误");
				public static readonly Status AuthException = new Status(11300, "异常登录");
				public static readonly Status AuthBlock = new Status(11400, "账号存在危险,已阻止");
				public static readonly Status AuthAccountOrPsw = new Status(11500, "账号或密码错误");
			}
			public static class Auth
			{
				public static class Invalid
				{
					public static readonly Status Default = new Status(12100, "账号权限不足");
					public static readonly Status Unknown = new Status(12110, "存在不存在的操作");
					public static readonly Status NotLogin = new Status(12120, "用户未登录");
					public static readonly Status ByMutilError = new Status(12130, "存在不存在的操作");
				}

				public static class Verify
				{
					public static readonly Status Default = new Status(12200, "验证码异常");
					public static readonly Status Invalid = new Status(12210, "验证码错误");
				}

				public static class AuthCode
				{
					public static readonly Status Default = new Status(12300, "授权码异常");
					public static readonly Status Invalid = new Status(12310, "授权码错误");
				}

				public static class Permission
				{
					public static readonly Status Default = new Status(12400, "授权异常");
					public static readonly Status Exist = new Status(12410, "用户已具有此权限");
					public static readonly Status NotExist = new Status(12420, "授权规则不存在");
				}
				
			}
			public static class Register { 
				public static readonly Status Default = new Status(13000, "用户已存在");
				public static readonly Status UserExist = new Status(13100, "用户已存在");
			}
		}

		public static class User
		{
			public static readonly Status Default = new Status(20000, "用户发生未知错误");
			public static readonly Status NotExist = new Status(21000, "用户不存在");
			public static readonly Status NoCompany = new Status(22000, "无归属单位");
			
		}


		public static class Company
		{
			public static readonly Status Default = new Status(30000, "单位发生未知错误");
			public static readonly Status NotExist = new Status(31000, "单位不存在");
			public static readonly Status NoneCompanyBelong = new Status(32000, "用户不属于任何一个单位");
			public static readonly Status CreateExisted = new Status(33000, "创建的单位已经存在");
		}

		public static class Apply
		{
			public static readonly Status Default = new Status(40000, "申请发生未知错误");
			public static readonly Status NotExist = new Status(42000, "申请不存在");

			public static class Operation
			{
				public static readonly Status Default = new Status(43000, "申请操作异常");
				public static readonly Status Invalid = new Status(43100, "申请中发现无效的操作");
				public static readonly Status AuditBegan = new Status(43100, "申请已处于审核状态中");
				public static readonly Status AuditCrash = new Status(43200, "当前存在正在进行的申请（审核中、审核通过状态）");

				public static class ToCompany
				{
					public static readonly Status Default = new Status(43300, "申请操作中出现单位异常");
					public static readonly Status NoneToSubmit = new Status(43310, "至少需要提交到一个单位进行审批");
					public static readonly Status NotExist = new Status(43320, "在申请中未发现此单位");
				}
				public static readonly Status AuditBeenAcceptedByOneCompany = new Status(43400, "当前申请已被审核，无法撤回");
				public static readonly Status AuditIsPublic = new Status(43500, "当前申请处于发布状态，请先撤回申请");

			}

		}





	}

	public struct Status
	{
		public int status;
		public string message;

		public Status(int status, string message)
		{
			this.status = status;
			this.message = message;
		}
	}
}
