using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Extensions
{
	public static class ApplyExtensions
	{
		private static  Dictionary<int,AuditStatusMessage> statusDic { get; set; }
		public static Dictionary<int, AuditStatusMessage> StatusDic => statusDic ?? (statusDic = InitStatusDic());
		public static Dictionary<int,Color> StatusColors { get; set; }


		private static Dictionary<int, AuditStatusMessage> InitStatusDic()
		{
			StatusColors = new Dictionary<int, Color>
			{
				{(int)AuditStatus.NotPublish, Color.DarkGray},
				{(int)AuditStatus.Auditing, Color.Coral},
				{(int)AuditStatus.Withdrew, Color.Gray},
				{(int)AuditStatus.AcceptAndWaitAdmin, Color.DeepSkyBlue},
				{(int)AuditStatus.Accept, Color.LawnGreen},
				{(int)AuditStatus.Denied, Color.Red}
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
					Message = fieldInfo.Name
				});
			}

			return statusMessages;
		}
		public static ApplyDTO ToSummaryDTO(this ApplyAllDataDTO all)
		{
			return new ApplyDTO()
			{
				Company = all.Company,
				Create = all.Create,
				From = all.From,
				FromUserName = all.FromUserName,
				Id = all.Id,
				Current = all.Current,
				Status = all.Status
			};
		}
		public static ApplyDTO ToSummaryDTO(this Apply item)
		{
			var all= ToAllDataDTO(item);
			return all.ToSummaryDTO();
		}
		
		public static ApplyAllDataDTO ToAllDataDTO(this Apply item)
		{
			if (item == null) return null;
			var apply=new ApplyAllDataDTO()
			{
				Company = item.Company,
				Create = item.Create,
				From = item.From.RealName,
				FromUserName = item.From.UserName,
				Id = item.Id,
				
				Detail = new ApplyDetailDTO()
				{
					FromId = item.From.Id,
					Request = item.Request,
					Stamp = item.stamp,
					Xjlb = item.xjlb,
					Reason = item.Reason
				},
				Status = item.Status,
			};
			var list=item.Response.Select(res => new ApplyResponseDTO()
			{
				Id = res.Id,
				AuditBy = res.AuditingBy?.RealName,
				AuditUserName = res.AuditingBy?.UserName,
				Company = res.Company.Name,
				CompanyPath = res.Company.Path,
				HdlStamp = res.HandleStamp,
				Remark = res.Remark,
				Status = res.Status
			});
			apply.Progress = list;
			apply.Current=list.FirstOrDefault(p => p.Status == Auditing.Received)?.Company;
			return apply;
		}
	}
}
