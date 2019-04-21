using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Extensions
{
	public static class ApplyExtensions
	{
		//public static Dictionary<int, string> StatusDic { get; set; }
		//private static bool Init=false;
		//static ApplyExtensions()
		//{
			
		//}

		//private static void Initilize()
		//{
		//	StatusDic = new Dictionary<int, string>();
		//	var type = typeof(AuditStatus).GetFields();
		//	foreach (var fieldInfo in type)
		//	{
		//		StatusDic.Add((int)fieldInfo.GetRawConstantValue(), fieldInfo.Name);
		//	}
		//}
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
