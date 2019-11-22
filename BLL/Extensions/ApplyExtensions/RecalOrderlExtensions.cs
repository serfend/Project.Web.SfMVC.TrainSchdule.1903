using DAL.DTO.Recall;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions.ApplyExtensions
{
	public static class RecalOrderlExtensions
	{
		public static RecallOrderVDto ToVDto(this RecallOrder model)
		{
			if (model == null) return null;
			return new RecallOrderVDto()
			{
				ReturnStamp = model.ReturnStramp,
				Create = model.Create,
				Apply = model.Apply.ToSummaryDto(model.RecallBy.Id)
			};
		}
	}
}
