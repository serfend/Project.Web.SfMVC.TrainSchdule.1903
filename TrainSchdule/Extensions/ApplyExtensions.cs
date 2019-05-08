﻿using System;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.Apply;
using TrainSchdule.ViewModels.Apply;

namespace TrainSchdule.Extensions
{
	public static class ApplyExtensions
	{
		public static ApplyBaseInfoVdto ToVDTO(this SubmitBaseInfoViewModel model,IUsersService usersService)
		{
			var b=new ApplyBaseInfoVdto()
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

		public static ApplyRequestVdto ToVDTO(this SubmitRequestInfoViewModel model,ApplicationDbContext context)
		{
			var b=new ApplyRequestVdto()
			{
				OnTripLength = model.OnTripLength,
				Reason = model.Reason,
				StampLeave = model.StampLeave,
				StampReturn = model.StampLeave?.AddDays(model.OnTripLength).AddDays(model.VocationLength),
				VocationLength = model.VocationLength,
				VocationPlace = context.AdminDivisions.Find(model.VocationPlace),
				VocationType = model.VocationType
			};
			return b;
		}

		public static ApplyVdto ToVDTO(this SubmitApplyViewModel model)
		{
			var b=new ApplyVdto()
			{
				BaseInfoId = model.BaseId??Guid.Empty,
				RequestInfoId = model.RequestId ?? Guid.Empty
			};
			return b;
		}

		public static ApplyAuditVdto ToAuditVDTO(this AuditApplyViewModel model,IUsersService usersService,IApplyService applyService)
		{
			var b=new ApplyAuditVdto()
			{
				Action = model.Data.Action,
				Apply = applyService.Get(model.Data.Id),
				AuditUser = usersService.Get(model.Auth.AuthByUserID),
				Remark = model.Data.Remark
			};
			return b;
		}
	}
}
