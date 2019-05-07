using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;

namespace BLL.Extensions
{
	public static class ApplyExtensions
	{
		public static ApplyBaseInfoDTO ToDTO(this ApplyBaseInfo model)
		{
			return new ApplyBaseInfoDTO()
			{
				CompanyName=model.CompanyName,
				DutiesName = model.DutiesName,
				RealName = model.RealName
			};
		}
		public static ApplySummaryDTO ToDTO(this Apply model)
		{
			var b=new ApplySummaryDTO()
			{
				Create = model.Create,
				Status = model.Status,
				NowAuditCompany = model.Response.FirstOrDefault(r=>r.Status==Auditing.Received||r.Status==Auditing.Denied)?.Company.Name,
				Base = model.BaseInfo.ToDTO(),
				
				Id = model.Id,
				StampLeave = model.RequestInfo.StampLeave,
				StampReturn = model.RequestInfo.StampReturn,
				VocationPlace = model.RequestInfo.VocationPlace.Name,
				HomePlace=model.BaseInfo.Social.Address.Name
			};
			return b;
		}
		private static  Dictionary<int,AuditStatusMessage> statusDic { get; set; }
		public static Dictionary<int, AuditStatusMessage> StatusDic => statusDic ?? (statusDic = InitStatusDic());
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
			for (int i=1;i<type.Length ;i++)
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
