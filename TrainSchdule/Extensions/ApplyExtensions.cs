using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.DTO.Apply;
using TrainSchdule.ViewModels.Apply;

namespace TrainSchdule.Extensions
{
	public static class ApplyExtensions
	{
		public static ApplyBaseInfoDTO ToDTO(this SubmitBaseInfoViewModel model,IUsersService usersService)
		{
			var b=new ApplyBaseInfoDTO()
			{
				Company = model.Company,
				Duties = model.Duties,
				From = usersService.Get(model.Id),
				HomeAddress = model.HomeAddress,
				HomeDetailAddress = model.HomeDetailAddress,
				Phone = model.Phone,
				RealName = model.RealName,
				Settle = model.Settle,
			};
			return b;
		}
	}
}
