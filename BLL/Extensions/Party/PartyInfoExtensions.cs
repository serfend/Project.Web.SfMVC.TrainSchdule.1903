using DAL.Data;
using DAL.DTO.ZZXT;
using DAL.Entities.UserInfo;
using DAL.Entities.ZZXT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions.Party
{
    public static class PartyInfoExtensions
    {
        public static UserPartyInfoDto ToDto(this UserPartyInfo model)
        {
            var user = model.User;
            var companyInfo = user.CompanyInfo;
            return new UserPartyInfoDto()
            {
                Id = model.Id,
                Appotime = model.DutyStart,
                Dilabo = model.Duty.Name,
                DxzNum = model.PartyGroupId,
                Number = model.CompanyCode,
                IsRemoved = model.IsRemoved,
                IsRemovedDate = model.IsRemovedDate,
                Posichan = model.Create,
                Post = $"{companyInfo.Company.Name}{companyInfo.Duties.Name}",
                Rudtime = user.BaseInfo.Time_Party,
                Ruwutime = user.BaseInfo.Time_Work,
                UserName = user.Id,
                Zzmm = model.TypeInParty,
            };
        }
        public static UserPartyInfo ToModel(this UserPartyInfoDto model, ApplicationDbContext context, UserPartyInfo raw = null)
        {
            if (raw == null) raw = new UserPartyInfo();
            raw.Id = model.Id;
            raw.DutyStart = model.Appotime;
            raw.Duty = context.PartyDuties.FirstOrDefault(d => d.Name == model.Dilabo);
            raw.PartyGroup = context.PartyGroups.FirstOrDefault(g => g.Id == model.DxzNum);
            raw.Company = context.CompaniesDb.FirstOrDefault(c => c.Code == model.Number);
            raw.Create = model.Posichan;
            raw.UserName = model.UserName;
            raw.TypeInParty = model.Zzmm;
            return raw;
        }
    }
}
