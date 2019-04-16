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
					Xjlb = item.xjlb
				},
			};
			var list = new List<ApplyResponseDTO>(item.Response.Count());
			bool AllPass=item.Response.All(res =>
			{
				var resDTO = new ApplyResponseDTO()
				{
					Id = res.Id,
					AuditBy = res.AuditingBy?.RealName,
					AuditUserName = res.AuditingBy?.UserName,
					Company = res.Company.Name,
					CompanyPath = res.Company.Path,
					HdlStamp = res.HandleStamp,
					Remark = res.Remark,
					Status = res.Status
				};
				list.Add(resDTO);
				switch (res.Status)
				{
					case Auditing.Accept:
					{
						break;
					}

					case Auditing.Received:
					{
						apply.Current = res.Company.Name;
						break;
					}
					case Auditing.Denied:
						apply.Current = res.Company.Name;
						apply.Status = AuditStatus.Denied;
						break;
					case Auditing.UnReceive:
					{
						apply.Current = res.Company.Name;
						apply.Status = AuditStatus.Auditing;
						break;
					}
				}
				return true;
			});
			apply.Progress = list;

			return apply;
		}
	}
}
