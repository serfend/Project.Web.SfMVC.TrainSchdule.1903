using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BLL.Extensions
{
	public static class ApplyExtensions
	{
		public static string ToStr(this Auditing model)
		{
			switch (model)
			{
				case Auditing.Received: return "审核中";
				case Auditing.Denied: return "驳回";
				case Auditing.UnReceive: return "未收到审核";
				case Auditing.Accept: return "同意";
				default: return "null";
			}
		}

		public static ApplyResponseDto SelfRankAuditStatus(this IEnumerable<ApplyResponseDto> model)
		{
			var list = model.ToList();
			if (list?.Count == 0) return null;
			var item = list.ElementAtOrDefault(0);
			return item;
		}
		public static ApplyResponseDto LastRankAuditStatus(this IEnumerable<ApplyResponseDto> model)
		{
			var list = model.ToList();

			if (list?.Count<2 ) return null;
			var item = list.ElementAtOrDefault(list.Count-2);
			return item;
		}

		public static string AuditResult(this ApplyResponseDto model)
		{
			if (model == null) return "无审批结果";
			var remark = model.Remark!=null ? $"({model.Remark})" : null;
			return $"{model.Status.ToStr()}{remark}";
		}
		public static int VocationTotalLength(this ApplyRequest model)
		{
			if (model?.StampReturn == null||!model.StampLeave.HasValue) return 0;
			return model.StampReturn.Value.Subtract(model.StampLeave.Value).Days;
		}
		public static string VocationDescription(this ApplyRequest model)
		{
			if (model == null) return "休假申请无效";
			return $"共计{model.OnTripLength+model.VocationLength}天(其中{model.VocationType}{model.VocationLength}天,路途{model.OnTripLength}天)";
		}
		public static ApplyBaseInfoDto ToDto(this ApplyBaseInfo model)
		{
			return new ApplyBaseInfoDto()
			{
				CompanyName=model?.CompanyName,
				DutiesName = model.DutiesName,
				RealName = model.RealName
			};
		}
		public static ApplyResponseDto ToResponseDto(this ApplyResponse model)
		{
			var b = new ApplyResponseDto()
			{
				AuditingUserRealName = model?.AuditingBy?.BaseInfo?.RealName,
				CompanyName = model.Company.Name,
				HandleStamp = model.HandleStamp,
				Remark = model.Remark,
				Status = model.Status
			};
			return b;
		}
		public static ApplyDetailDto ToDetaiDto(this Apply model)
		{
			var b=new ApplyDetailDto()
			{
				Base = model?.BaseInfo.From.ToSummaryDto(),
				Company = model.BaseInfo.Company,
				Create = model.Create,
				Duties = model.BaseInfo.Duties,
				Hidden = model.Hidden,
				RequestInfo = model.RequestInfo,
				Response = model.Response.Select(r=>r.ToResponseDto()),
				Id = model.Id,
				Social = model.BaseInfo.Social,
				Status = model.Status,
				AuditLeader = model.AuditLeader
			};
			return b;
		}

		
		public static ApplySummaryDto ToSummaryDto(this Apply model,string auditFrom)
		{
			var b=new ApplySummaryDto()
			{
				Create = model?.Create,
				Status = model.Status,
				NowAuditCompany = model.Response.FirstOrDefault(r=>r.Status==Auditing.Received||r.Status==Auditing.Denied)?.Company.Name,
				Base = model.BaseInfo.ToDto(),
				UserBase = model.BaseInfo.From.ToSummaryDto(),
				Id = model.Id,
				Request=model.RequestInfo,
				AuditAvailable = model.Response?.FirstOrDefault(r=>r.Status==Auditing.Received)?.Company.Code== auditFrom
			};
			return b;
		}

		public static Dictionary<int, AuditStatusMessage> StatusDic { get; } = InitStatusDic();
		public static Dictionary<int,Color> StatusColors { get; set; }
		public static Dictionary<int,string> StatusDesc { get; set; }
		public static Dictionary<int, IEnumerable<string>> StatusAcessable { get; set; }


		private static Dictionary<int, AuditStatusMessage> InitStatusDic()
		{
			StatusColors = new Dictionary<int, Color>
			{
				{(int)AuditStatus.NotPublish, Color.DarkGray},
				{(int)AuditStatus.Auditing, Color.Coral},
				{(int)AuditStatus.Withdrew, Color.Gray},
				{(int)AuditStatus.AcceptAndWaitAdmin, Color.DeepSkyBlue},
				{(int)AuditStatus.Accept, Color.LawnGreen},
				{(int)AuditStatus.Denied, Color.Red},
				{(int)AuditStatus.NotSave,Color.Black }
			};
			StatusDesc=new Dictionary<int, string>()
			{
				{(int)AuditStatus.NotPublish, "未发布"},
				{(int)AuditStatus.Auditing, "审核中"},
				{(int)AuditStatus.Withdrew, "已撤回"},
				{(int)AuditStatus.AcceptAndWaitAdmin, "人力审核"},
				{(int)AuditStatus.Accept, "已通过"},
				{(int)AuditStatus.Denied, "已驳回"},
				{(int)AuditStatus.NotSave,"未保存" }
			};
			StatusAcessable=new Dictionary<int, IEnumerable<string>>()
			{
				{(int)AuditStatus.NotPublish,new List<string>(){"Publish","Delete"}},
				{(int)AuditStatus.Auditing, new List<string>(){"Withdrew","Delete"}},
				{(int)AuditStatus.Withdrew, new List<string>(){"Delete"}},
				{(int)AuditStatus.AcceptAndWaitAdmin, new List<string>(){"Withdrew","Delete"}},
				{(int)AuditStatus.Accept, new List<string>(){""}},
				{(int)AuditStatus.Denied, new List<string>(){"Delete"}},
				{(int)AuditStatus.NotSave,new List<string>(){"Delete","Save","Publish"} }
			};
			var statusMessages = new Dictionary<int, AuditStatusMessage>();
			var type = typeof(AuditStatus).GetFields();
			for (var i=1;i<type.Length ;i++)
			{
				var fieldInfo = type[i];
				var key =(int)fieldInfo.GetRawConstantValue();
				statusMessages.Add(key, new AuditStatusMessage()
				{
					Code = key,
					Color = ColorTranslator.ToHtml(Color.FromArgb(StatusColors[key].ToArgb())),
					Message = fieldInfo.Name,
					Desc = StatusDesc[key],
					Acessable = StatusAcessable[key]
				});
			}

			return statusMessages;
		}
	}
}
