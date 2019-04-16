using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSchdule.BLL.Helpers
{
	public static class ActionStatusMessage
	{
		public static readonly Status Success=new Status(0,null);

		public static readonly Status AccountLogin_InvalidAuth=new Status(600,"登录验证失败");
		public static readonly Status AccountLogin_InvalidByUnknown=new Status(60000,"存在异常导致登录失败");
		public static readonly Status AccountLogin_InvalidAuthFormat=new Status(60001,"输入格式错误");
		public static readonly Status AccountLogin_InvalidAuthException=new Status(60002,"异常登录");
		public static readonly Status AccountLogin_InvalidAuthBlock=new Status(60004,"账号存在危险,已阻止");
		public static readonly Status AccountLogin_InvalidAuthAccountOrPsw=new Status(60008,"账号或密码错误");
		public static readonly Status AccountLogin_InvalidVerifyCode = new Status(60009, "验证码错误");

		public static readonly Status AccountAuth_Forbidden = new Status(403,"账号权限不足");
		public static readonly Status AccountAuth_InvalidByMutilError=new Status(40302,"存在不存在的操作");
		public static readonly Status AccountAuth_Invalid = new Status(40301,"登录凭证已失效");

		public static readonly Status AccountRegister_UserExist = new Status(70001,"用户已存在");

		public static readonly Status User_Unknown = new Status(80, "用户发生未知错误");
		public static readonly Status User_NotExist = new Status(80404, "用户不存在");


		public static readonly Status Company_Unknown = new Status(140, "单位发生未知错误");
		public static readonly Status Company_NotExist = new Status(140404,"单位不存在");
		public static readonly Status Company_NoneCompanyBelong = new Status(140002,"用户不属于任何一个单位");
		public static readonly Status Company_CreateExisted = new Status(140004,"创建的单位已经存在");


		public static readonly Status Apply_Unknow = new Status(150, "申请发生未知错误");
		public static readonly Status Apply_NoCompanyToSubmit = new Status(150001, "至少需要提交到一个单位进行审批");
		public static readonly Status Apply_NotExist = new Status(150404, "申请不存在");
		public static readonly Status Apply_OperationInvalid = new Status(150004, "申请不存在");
		public static readonly Status Apply_OperationAuditBegan = new Status(150005, "申请已处于审核状态中");
		public static readonly Status Apply_OperationAuditCrash = new Status(150006, "当前存在正在进行的申请（审核中、审核通过状态）");




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
