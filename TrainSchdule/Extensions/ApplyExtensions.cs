using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Extensions;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.Apply;
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
		public static ApplyBaseInfoVdto ToVDTO(this SubmitBaseInfoViewModel model,IUsersService usersService)
		{
			var b=new ApplyBaseInfoVdto()
			{
				Company = model.Company,
				Duties = model.Duties,
				From = usersService.Get(model.Id),
				//TODO 【Future unnecessary】可能可以利用为休假去向
				//VocationTargetAddress=model.VocationTargetAddress,
				//VocationTargetAddressDetail=model.VocationTargetAddressDetail,
				Phone = model.Phone,
				RealName = model.RealName,
				Settle = model.Settle,
			};
			return b;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <param name="context"></param>
		/// <param name="vocationCheckServices"></param>
		/// <returns></returns>
		public static ApplyRequestVdto ToVDTO(this SubmitRequestInfoViewModel model,ApplicationDbContext context,IVocationCheckServices vocationCheckServices)
		{
			var b=new ApplyRequestVdto()
			{
				OnTripLength = model.OnTripLength,
				Reason = model.Reason,
				StampLeave = model.StampLeave,
				VocationLength = model.VocationLength,
				VocationPlace = context.AdminDivisions.Find(model.VocationPlace),
				VocationType = model.VocationType,
				ByTransportation = model.ByTransportation
			};
			if (b.StampLeave != null)
			{
				b.StampReturn = vocationCheckServices.CrossVocation(b.StampLeave.Value, b.OnTripLength + b.VocationLength);
				b.VocationDescriptions = vocationCheckServices.VocationDesc.ToDescription(); 
			}
			return b;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static ApplyVdto ToVDTO(this SubmitApplyViewModel model)
		{
			var b=new ApplyVdto()
			{
				BaseInfoId = model.BaseId??Guid.Empty,
				RequestInfoId = model.RequestId ?? Guid.Empty
			};
			return b;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <param name="usersService"></param>
		/// <param name="applyService"></param>
		/// <returns></returns>
		public static ApplyAuditVdto ToAuditVDTO(this AuditApplyViewModel model,IUsersService usersService,IApplyService applyService)
		{
			var user = usersService.Get(model.Auth.AuthByUserID);
			var b = new ApplyAuditVdto()
			{
				AuditUser = user,
				List = model.Data.List.Select(d => new ApplyAuditNodeVdto()
				{
					Action = d.Action,
					Apply = applyService.Get(d.Id),
					Remark = d.Remark
				})

			};
			return b;
		}
	}
}
