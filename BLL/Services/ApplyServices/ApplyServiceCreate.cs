using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Linq.Expressions;
using BLL.Extensions;
using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Audit;
using BLL.Services.Audit;
using DAL.Data;
using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.Vacations;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.ApplyServices
{
	public class ApplyServiceCreate: IApplyServiceCreate
	{
        private readonly ApplicationDbContext context;
		public ApplyServiceCreate(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<ApplyBaseInfo> SubmitBaseInfoAsync(ApplyBaseInfoVdto model)
		{
			if (model == null) return null;
			var m = new ApplyBaseInfo()
			{
				Company = await context.CompaniesDb.FirstOrDefaultAsync(c => c.Code == model.Company).ConfigureAwait(true),
				Duties = await context.Duties.FirstOrDefaultAsync(d => d.Name == model.Duties).ConfigureAwait(true),
				From = model.From,
				CreateBy = model.CreateBy,
				Social = new UserSocialInfo()
				{
					Address = await context.AdminDivisions.FindAsync(model.VacationTargetAddress).ConfigureAwait(true),
					AddressDetail = model.VacationTargetAddressDetail,
					Phone = model.Phone,
					Settle = model.Settle // 此处可能需要静态化处理，但考虑到.History问题，再议
				},
				RealName = model.RealName,
				CompanyName = model.Company,
				DutiesName = model.Duties,
				CreateTime = DateTime.Now
			};
			if (m.Company != null) m.CompanyName = m.Company.Name;
			await context.ApplyBaseInfos.AddAsync(m).ConfigureAwait(true);
			await context.SaveChangesAsync().ConfigureAwait(true);
			return m;
		}

	}
}