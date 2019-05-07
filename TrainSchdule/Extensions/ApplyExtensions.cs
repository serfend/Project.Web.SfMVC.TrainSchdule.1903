using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.Apply;
using TrainSchdule.ViewModels.Apply;

namespace TrainSchdule.Extensions
{
	public static class ApplyExtensions
	{
		public static ApplyBaseInfoVDTO ToVDTO(this SubmitBaseInfoViewModel model,IUsersService usersService)
		{
			var b=new ApplyBaseInfoVDTO()
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

		public static ApplyRequestVDTO ToVDTO(this SubmitRequestInfoViewModel model,ApplicationDbContext context)
		{
			var b=new ApplyRequestVDTO()
			{
				OnTripLength = model.OnTripLength,
				Reason = model.Reason,
				StampLeave = model.StampLeave,
				StampReturn = model.StampLeave.AddDays(model.OnTripLength).AddDays(model.VocationLength),
				VocationLength = model.VocationLength,
				VocationPlace = context.AdminDivisions.Find(model.VocationPlace),
				VocationType = model.VocationType
			};
			return b;
		}

		public static ApplyVDTO ToVDTO(this SubmitApplyViewModel model)
		{
			var b=new ApplyVDTO()
			{
				BaseInfoId = model.BaseId??Guid.Empty,
				RequestInfoId = model.RequestId ?? Guid.Empty
			};
			return b;
		}
	}
}
