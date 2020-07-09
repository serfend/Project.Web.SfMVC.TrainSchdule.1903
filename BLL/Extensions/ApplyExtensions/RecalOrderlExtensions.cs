using DAL.DTO.Recall;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;

namespace BLL.Extensions.ApplyExtensions
{
	public static class RecalOrderlExtensions
	{
		public static HandleByVdto ToVDto<T>(this T model, Apply apply) where T : HandleModifyReturnStamp
		{
			if (model == null) return null;
			return new HandleByVdto()
			{
				Apply = apply == null ? Guid.Empty : apply.Id,
				ReturnStamp = model.ReturnStamp,
				Create = model.Create,
				Reason = model.Reason,
				HandleBy = model.HandleBy.ToSummaryDto()
			};
		}
	}
}