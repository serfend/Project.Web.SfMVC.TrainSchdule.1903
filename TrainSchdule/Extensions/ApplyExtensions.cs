﻿using BLL.Extensions;
using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.Vacations;
using System;
using System.Collections.Generic;
using System.Linq;
using TrainSchdule.ViewModels.Apply;

namespace TrainSchdule.Extensions
{
	/// <summary>
	///
	/// </summary>
	public static class ApplyExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <param name="usersService"></param>
		/// <returns></returns>
		public static ApplyBaseInfoVdto ToVDTO(this SubmitBaseInfoViewModel model, IUsersService usersService)
		{
			var b = new ApplyBaseInfoVdto()
			{
				Company = model.Company,
				Duties = model.Duties,
				From = usersService.GetById(model.Id),
				//TODO 【Future unnecessary】可能可以利用为休假去向
				//VacationTargetAddress=model.VacationTargetAddress,
				//VacationTargetAddressDetail=model.VacationTargetAddressDetail,
				Phone = model.Phone,
				RealName = model.RealName,
				Settle = model.Settle
			};
			return b;
		}
		/// <summary>
		///  转换并计算用户的申请
		/// </summary>
		/// <param name="model">原始申请</param>
		/// <param name="context">数据库</param>
		/// <returns></returns>
		public static ApplyRequestVdto ToVDTO(this SubmitRequestInfoViewModel model, ApplicationDbContext context)
		{
			var successVacationPlace = int.TryParse(model.VacationPlace, out var vacationPlace);
			var b = new ApplyRequestVdto()
			{
				Reason = model.Reason,				
				StampLeave = model.StampLeave,
				OnTripLength = model.OnTripLength,
				VacationLength = model.VacationLength,
				VacationPlace = context.AdminDivisions.Where(a => a.Code == vacationPlace).FirstOrDefault(),
				VacationPlaceName = model.VacationPlaceName,
				VacationType = context.VacationTypes.Where(t => t.Name == model.VacationType).FirstOrDefault(),
				ByTransportation = model.ByTransportation,
				VacationAdditionals = model.VacationAdditionals,
				LawVacationSet = model.LawVacationSet,
				IsPlan=model.IsPlan??false
			};
			return b;
		}
		/// <summary>
		///  转换并计算用户的申请
		/// </summary>
		/// <param name="model">原始申请</param>
		/// <param name="context">数据库</param>
		/// <returns></returns>
		public static ApplyIndayRequestVdto ToVDTO(this SubmitIndayRequestInfoViewModel model, ApplicationDbContext context)
		{
			var successVacationPlace = int.TryParse(model.VacationPlace, out var vacationPlace);
			var b = new ApplyIndayRequestVdto()
			{
				RequestType=context.VacationIndayTypes.FirstOrDefault(i=>i.Name == model.RequestType),
				Reason = model.Reason,
				StampLeave = model.StampLeave,
				VacationPlace = context.AdminDivisions.Where(a => a.Code == vacationPlace).FirstOrDefault(),
				VacationPlaceName = model.VacationPlaceName,
				ByTransportation = model.ByTransportation,
				StampReturn = model.StampReturn,
			};
			return b;
		}
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static ApplyVdto ToVDTO(this SubmitApplyViewModel model)
		{
			var b = new ApplyVdto()
			{
				BaseInfoId = model.BaseId ?? Guid.Empty,
				RequestInfoId = model.RequestId ?? Guid.Empty,
				IsPlan = model.IsPlan,
				EntityType=model.EntityType
			};
			return b;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <param name="auditUser"></param>
		/// <param name="db"></param>
		/// <returns></returns>
		public static ApplyAuditVdto<AuditStreamModel> ToAuditVDTO<T>(this AuditApplyViewModel model, User auditUser, IQueryable<T> db) where T:IHasGuidId,IAuditable,new()
		{
			var b = new ApplyAuditVdto<AuditStreamModel>()
			{
				AuditUser = auditUser,
				List = model.Data.List.Select(d => new ApplyAuditNodeVdto<AuditStreamModel>()
				{
					Action = d.Action,
					AuditItem = db.FirstOrDefault(i => i.Id == d.Id).ToModel(),
					Remark = d.Remark
				})
			};
			return b;
		}
	}
}