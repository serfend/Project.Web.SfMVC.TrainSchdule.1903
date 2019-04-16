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
		public static ApplyDTO ToDTO(this Apply item)
		{
			var apply=new ApplyDTO()
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
					AuditBy = res.AuditingBy?.RealName,
					AuditUserName = res.AuditingBy?.UserName,
					Company = res.Company.Name,
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
						return false;
					}
					case Auditing.Declined:
						apply.Current = res.Company.Name;
						apply.Status = AuditStatus.Declined;
						return false;
					case Auditing.UnReceive:
					{
						apply.Current = res.Company.Name;
						apply.Status = AuditStatus.Auditing;
						return false;
					}
				}
				return true;
			});
			apply.Progress = list;

			return apply;
		}
	}
}
