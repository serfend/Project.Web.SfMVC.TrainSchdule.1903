using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Extensions;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.Apply;
using DAL.Entities.Vocations;
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
				From = usersService.Get(model.Id),
				//TODO 【Future unnecessary】可能可以利用为休假去向
				//VocationTargetAddress=model.VocationTargetAddress,
				//VocationTargetAddressDetail=model.VocationTargetAddressDetail,
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
		/// <param name="vocationCheckServices">假期计算服务</param>
		/// <param name="CaculateAdditionalAndTripLength">是否计算福利假和路途</param>
		/// <returns></returns>
		public static ApplyRequestVdto ToVDTO(this SubmitRequestInfoViewModel model, ApplicationDbContext context, IVocationCheckServices vocationCheckServices, bool CaculateAdditionalAndTripLength)
		{
			var b = new ApplyRequestVdto()
			{
				OnTripLength = model.OnTripLength,
				Reason = model.Reason,
				StampLeave = model.StampLeave,
				VocationLength = model.VocationLength,
				VocationPlace = context.AdminDivisions.Find(model.VocationPlace),
				VocationType = model.VocationType,
				ByTransportation = model.ByTransportation,
				VocationAdditionals = model.VocationAdditionals
			};
			int additionalVocationDay = 0;
			b.VocationAdditionals?.All(v => { additionalVocationDay += v.Length; v.Start = DateTime.Now; return true; });
			if (b.StampLeave != null)
			{
				var vocationLength = b.VocationLength + (CaculateAdditionalAndTripLength ? (b.OnTripLength + additionalVocationDay) : 0);
				if (CaculateAdditionalAndTripLength)
				{
					b.StampReturn = vocationCheckServices.CrossVocation(b.StampLeave.Value, vocationLength, CaculateAdditionalAndTripLength);
					List<VocationAdditional> lawVocations = vocationCheckServices.VocationDesc.Select(v => new VocationAdditional()
					{
						Name = v.Name,
						Start = v.Start,
						Length = v.Length,
						Description = "法定节假日"
					}).ToList();
					lawVocations.AddRange(b.VocationAdditionals);
					b.VocationAdditionals= lawVocations;//执行完crossVocation后已经处于加载完毕状态可直接使用
				}
				else b.StampReturn = b.StampLeave.Value.AddDays(vocationLength-1);


				b.VocationDescriptions = vocationCheckServices.VocationDesc.CombineVocationDescription(CaculateAdditionalAndTripLength);
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
			var b = new ApplyVdto()
			{
				BaseInfoId = model.BaseId ?? Guid.Empty,
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
		public static ApplyAuditVdto ToAuditVDTO(this AuditApplyViewModel model, IUsersService usersService, IApplyService applyService)
		{
			var user = usersService.Get(model.Auth.AuthByUserID);
			var b = new ApplyAuditVdto()
			{
				AuditUser = user,
				List = model.Data.List.Select(d => new ApplyAuditNodeVdto()
				{
					Action = d.Action,
					Apply = applyService.GetById(d.Id),
					Remark = d.Remark
				})

			};
			return b;
		}
	}
}
