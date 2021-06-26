﻿using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;

namespace BLL.Extensions.ApplyExtensions
{
	public static class ApplyBaseInfoExtensions
	{
		public static ApplyBaseInfoDto ToDto(this ApplyBaseInfo model)
		{
			return new ApplyBaseInfoDto()
			{
				CompanyName = model?.CompanyName,
				DutiesName = model.DutiesName,
				RealName = model.RealName,
				UserId = model.FromId,
				Phone = model.Social.Phone
			};
		}
	}
}