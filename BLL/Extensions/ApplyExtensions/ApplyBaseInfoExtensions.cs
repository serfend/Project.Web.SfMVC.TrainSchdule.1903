using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Text;

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
				RealName = model.RealName
			};
		}
	}
}
