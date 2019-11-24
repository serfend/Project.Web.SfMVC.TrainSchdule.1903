﻿namespace BLL.Helpers
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
					public static readonly Status Default = new Status(12100, "访问被拒绝");
					public static readonly Status Unknown = new Status(12110, "存在不存在的操作");
					public static readonly Status NotLogin = new Status(12120, "用户未登录");
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
			public static readonly Status NotCorrectId=new Status(23000, "非法的用户身份号，仅可为7位身份号或18位身份证号");
			public static readonly Status NoId=new Status(24000,"未填写用户身份号");


		}


		public static class Company
		{
			public static readonly Status Default = new Status(30000, "单位发生未知错误");
			public static readonly Status NotExist = new Status(31000, "单位不存在");
			public static readonly Status NoneCompanyBelong = new Status(32000, "用户不属于任何一个单位");
			public static readonly Status CreateExisted = new Status(33000, "创建的单位已经存在");
			public static class Manager
			{
				public static readonly Status Default = new Status(34000, "单位主管发生未知错误");
				public static readonly Status NotExist=new Status(34100, "单位中不存在此主管");
				public static readonly Status Existed=new Status(34200, "单位中已存在此主管");
			}
		}

		public static class Apply
		{
			public static readonly Status Default = new Status(40000, "申请发生未知错误");
			public static readonly Status NotExist = new Status(42000, "申请不存在");

			public static class Operation
			{
				public static readonly Status Default = new Status(43000, "申请操作异常");
				public static readonly Status Invalid = new Status(43100, "对申请的操作无效");
			

				
				public static class  Withdrew
				{
					
					public static readonly Status AllReadyWithdrew = new Status(43220, "当前申请处于已撤回状态，无需撤回");
					public static readonly Status AuditBeenAcceptedByOneCompany = new Status(43230, "当前申请已被审核，无法撤回");
				}

				public static class Submit
				{
					public static readonly Status Crash = new Status(43310, "申请过于频繁");
					public static readonly Status Began = new Status(43320, "申请已处于审核状态中");
					public static readonly Status NoRequestInfo =new Status(43330,"申请的请求信息无效");
					public static readonly Status NoBaseInfo = new Status(43340, "申请的基础信息无效");
				}
				public static class ToCompany
				{
					public static readonly Status Default = new Status(43400, "申请操作中出现单位异常");
					public static readonly Status NoneToSubmit = new Status(43410, "至少需要提交到一个单位进行审批");
					public static readonly Status NotExist = new Status(43420, "在申请中未发现此单位");
				}

				public static class StatusInvalid
				{
					public static readonly Status NotOnPublishable = new Status(43510, "当前申请不处于可发布的状态");

					public static readonly Status NotOnAuditingStatus = new Status(43520, "当前申请不处于审核中状态");
					public static readonly Status NotOnNotSaveStatus = new Status(43530, "当前申请不处于未保存状态");
				}
				
				public static class Audit
				{
					public static readonly Status BeenAudit = new Status(43610, "审核已提交过，请勿重复审核");
					public static readonly Status NotExist = new Status(43620, "不存在的审核流程");
					public static readonly Status NoYourAuditStream=new Status(43630,"无用户审批的审批流");
					public static readonly Status BeenAuditOrNotReceived = new Status(43640,"已审核或未收到审核");
					

				}
				public static class Save
				{
					public static readonly Status AllReadySave = new Status(43710, "当前申请处于保存状态");

				}


			}

			public static class Request
			{
				public static readonly Status Default = new Status(44000, "无效的休假申请");
				public static readonly Status OutOfDate = new Status(44100, "申请的离队时间不可早于当前时间");
				public static readonly Status NoEnoughVocation = new Status(44200, "剩余休假天数不足");
				public static readonly Status TripTimesExceed = new Status(44300, "剩余可休路途次数不足");
				public static readonly Status VocationLengthTooShort = new Status(44400, "休假长度不可少于5天");
				
			}
			public static readonly Status GuidFail = new Status(45000, "申请的ID无效");
			public static class Recall
			{
				public static readonly Status NotExist = new Status(46100, "召回信息不存在");
				public static readonly Status RecallByNotSame = new Status(46200, "召回人和终审人不一致");
				public static readonly Status Crash = new Status(46300, "休假已存在召回记录");
				public static readonly Status IdRecordButNoData = new Status(46400, "用户休假申请的召回id存在，但无此召回的详细信息");
				public static readonly Status RecallTimeLateThenVocation = new Status(46500, "召回时间不可晚于（等于）休假结束时间");
			}
		}

		public static class Static
		{
			public static readonly Status TempXlsNotExist=new Status(50000,"未找到Excel模板");
			public static readonly Status XlsNoData = new Status(51000,"生成的Excel文件无数据");
			public static class AdminDivision
			{
				public static readonly Status NoSuchArea = new Status(52100, "无此地址");
				public static readonly Status NoChildArea = new Status(52200, "此地址无下属行政区");
			}
			
		}

		public static class Grade
		{
			public static class Subject
			{
				public static readonly Status NotExist = new Status(61100,"无此科目");
			}
		}
		public static class Statistics
		{
			public static readonly Status NotExist = new Status(71100, "无此id的统计数据");
			public static readonly Status AllreadyExist = new Status(71200, "此id的统计数据已存在");
			
		}
	}

	public class Status
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
