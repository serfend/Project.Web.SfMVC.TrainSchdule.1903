﻿namespace BLL.Helpers
{
	public static partial class ActionStatusMessage
	{
		public static class Apply
		{
			public static readonly ApiResult Default = new ApiResult(40000, "申请发生未知错误");
			public static readonly ApiResult NotExist = new ApiResult(42000, "申请不存在");

			public static class Operation
			{
				public static readonly ApiResult Default = new ApiResult(43000, "申请操作异常");
				public static readonly ApiResult Invalid = new ApiResult(43100, "对申请的操作无效");
			

				
				public static class  Withdrew
				{
					
					public static readonly ApiResult AllReadyWithdrew = new ApiResult(43220, "当前申请处于已撤回状态，无需撤回");
					public static readonly ApiResult AuditBeenAcceptedByOneCompany = new ApiResult(43230, "当前申请已被审核，无法撤回");
				}

				public static class Submit
				{
					public static readonly ApiResult Crash = new ApiResult(43310, "申请过于频繁");
					public static readonly ApiResult Began = new ApiResult(43320, "申请已处于审核状态中");
					public static readonly ApiResult NoRequestInfo =new ApiResult(43330,"申请的请求信息无效");
					public static readonly ApiResult NoBaseInfo = new ApiResult(43340, "申请的基础信息无效");
				}
				public static class ToCompany
				{
					public static readonly ApiResult Default = new ApiResult(43400, "申请操作中出现单位异常");
					public static readonly ApiResult NoneToSubmit = new ApiResult(43410, "至少需要提交到一个单位进行审批");
					public static readonly ApiResult NotExist = new ApiResult(43420, "在申请中未发现此单位");
				}

				public static class StatusInvalid
				{
					public static readonly ApiResult NotOnPublishable = new ApiResult(43510, "当前申请不处于可发布的状态");

					public static readonly ApiResult NotOnAuditingStatus = new ApiResult(43520, "当前申请不处于审核中状态");
					public static readonly ApiResult NotOnNotSaveStatus = new ApiResult(43530, "当前申请不处于未保存状态");
					public static readonly ApiResult CanNotDelete = new ApiResult(43540, "当前申请已受理，不可删除");

					
				}
				
				public static class Audit
				{
					public static readonly ApiResult BeenAudit = new ApiResult(43610, "审核已提交过，请勿重复审核");
					public static readonly ApiResult NotExist = new ApiResult(43620, "不存在的审核流程");
					public static readonly ApiResult NoYourAuditStream=new ApiResult(43630,"无用户审批的审批流");
					public static readonly ApiResult BeenAuditOrNotReceived = new ApiResult(43640,"已审核或未收到审核");
					

				}
				public static class Save
				{
					public static readonly ApiResult AllReadySave = new ApiResult(43710, "当前申请处于保存状态");

				}


			}

			public static class Request
			{
				public static readonly ApiResult Default = new ApiResult(44000, "无效的休假申请");
				// 可能为补充休假
				public static readonly ApiResult OutOfDate = new ApiResult(44100, "申请的离队时间不可早于当前时间");
				public static readonly ApiResult NoEnoughVocation = new ApiResult(44200, "剩余休假天数不足");
				public static readonly ApiResult TripTimesExceed = new ApiResult(44300, "剩余可休路途次数不足");
				public static readonly ApiResult VocationLengthTooShort = new ApiResult(44400, "休假长度不可少于5天");
				public static readonly ApiResult InvalidVocationType = new ApiResult(44500, "无效的休假类型");
				public static readonly ApiResult NotPermitCrossYear = new ApiResult(44600, "不允许跨年休假");
			}
			public static readonly ApiResult GuidFail = new ApiResult(45000, "申请的ID无效");
			public static class Recall
			{
				public static readonly ApiResult NotExist = new ApiResult(46100, "召回信息不存在");
				public static readonly ApiResult RecallByNotSame = new ApiResult(46200, "召回人和终审人不一致");
				public static readonly ApiResult Crash = new ApiResult(46300, "休假已存在召回记录");
				public static readonly ApiResult IdRecordButNoData = new ApiResult(46400, "用户休假申请的召回id存在，但无此召回的详细信息");
				public static readonly ApiResult RecallTimeLateThanVocation = new ApiResult(46500, "召回时间不可晚于（等于）休假结束时间");
				public static readonly ApiResult RecallTimeEarlyThanNow = new ApiResult(46600, "召回时间不可早于当前时间");
			}
		}
	}
}
