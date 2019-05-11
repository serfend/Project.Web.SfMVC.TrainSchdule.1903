using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BLL.Extensions
{
	public static class ApplyExtensions
	{
		public static int VocationTotalLength(this ApplyRequest model)
		{
			if (model == null) return 0;
			if (!model.StampReturn.HasValue||!model.StampLeave.HasValue) return 0;
			return model.StampReturn.Value.Subtract(model.StampLeave.Value).Days;
		}
		public static string VocationDescription(this ApplyRequest model)
		{
			if (model == null) return "休假申请无效";
			return $"共计{model.OnTripLength+model.VocationLength}天(其中{model.VocationType}{model.VocationLength}天,路途{model.OnTripLength}天)";
		}
		public static ApplyBaseInfoDto ToDTO(this ApplyBaseInfo model)
		{
			return new ApplyBaseInfoDto()
			{
				CompanyName=model.CompanyName,
				DutiesName = model.DutiesName,
				RealName = model.RealName
			};
		}
		public static ApplyResponseDto ToResponseDTO(this ApplyResponse model)
		{
			var b = new ApplyResponseDto()
			{
				AuditingUserRealName = model.AuditingBy?.BaseInfo?.RealName,
				CompanyName = model.Company.Name,
				HandleStamp = model.HandleStamp,
				Remark = model.Remark,
				Status = model.Status
			};
			return b;
		}
		public static ApplyDetailDto ToDetaiDTO(this Apply model)
		{
			var b=new ApplyDetailDto()
			{
				Base = model.BaseInfo.From.ToDTO(),
				Company = model.BaseInfo.Company,
				Create = model.Create,
				Duties = model.BaseInfo.Duties,
				Hidden = model.Hidden,
				RequestInfo = model.RequestInfo,
				Response = model.Response.Select(r=>r.ToResponseDTO()),
				Id = model.Id,
				Social = model.BaseInfo.Social,
				Status = model.Status
			};
			return b;
		}

		
		public static ApplySummaryDto ToSummaryDTO(this Apply model)
		{
			var b=new ApplySummaryDto()
			{
				Create = model.Create,
				Status = model.Status,
				NowAuditCompany = model.Response.FirstOrDefault(r=>r.Status==Auditing.Received||r.Status==Auditing.Denied)?.Company.Name,
				Base = model.BaseInfo.ToDTO(),
				UserBase = model.BaseInfo.From.ToDTO(),
				Id = model.Id,
				StampLeave = model.RequestInfo?.StampLeave,
				StampReturn = model.RequestInfo?.StampReturn,
				VocationPlace = model.RequestInfo?.VocationPlace.Name,
				HomePlace=model.BaseInfo.Social.Address.Name
			};
			return b;
		}
		private static  Dictionary<int,AuditStatusMessage> _statusDic { get; set; }
		public static Dictionary<int, AuditStatusMessage> StatusDic => _statusDic ?? (_statusDic = InitStatusDic());
		public static Dictionary<int,Color> StatusColors { get; set; }
		public static Dictionary<int,string> StatusDesc { get; set; }


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
				{(int)AuditStatus.AcceptAndWaitAdmin, "通过,待管理员审核"},
				{(int)AuditStatus.Accept, "已通过"},
				{(int)AuditStatus.Denied, "已驳回"},
				{(int)AuditStatus.NotSave,"未保存" }
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
					Desc = StatusDesc[key]
				});
			}

			return statusMessages;
		}
	}
}
