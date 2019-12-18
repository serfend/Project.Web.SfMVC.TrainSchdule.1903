using DAL.DTO.Recall;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions.ApplyExtensions
{
	public static class RecalOrderlExtensions
	{
		public static RecallOrderVDto ToVDto(this RecallOrder model,Apply apply,IHostingEnvironment env)
		{
			if (model == null) return null;
			return new RecallOrderVDto()
			{
				Apply = apply==null?Guid.Empty: apply.Id,
				ReturnStamp = model.ReturnStramp,
				Create = model.Create,
				Reason=model.Reason,
				RecallBy=model.RecallBy.ToSummaryDto(env)
			};
		}
	}
}
