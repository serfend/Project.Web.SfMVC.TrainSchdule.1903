using DAL.Data;
using DAL.DTO.ZZXT;
using DAL.Entities.ZZXT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions.Party
{
    public static class PartyGroupExtensions
    {
        public static PartyGroupDto ToDto(this PartyGroup model)
        {
            return new PartyGroupDto()
            {
                Alias = model.Alias,
                Company = model.CompanyCode,
                Create = model.Create,
                GroupType = model.GroupType,
            };
        }

        public static PartyGroup ToModel(this PartyGroupDto model, ApplicationDbContext context, PartyGroup raw = null)
        {
            if (raw == null) raw = new PartyGroup();
            raw.Alias = model.Alias;
            raw.Company = context.CompaniesDb.FirstOrDefault(g => g.Code == model.Company);
            if (raw.Company != null) raw.CompanyCode = model.Company;
            raw.Create = model.Create;
            raw.GroupType = model.GroupType;
            raw.Id = model.Id;
            return raw;
        }
    }
}
