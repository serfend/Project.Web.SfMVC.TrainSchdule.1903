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
                   Alias=model.Alias,
                   Company= model.CompanyCode,
                   Create=model.Create,
                   GroupType=model.GroupType,
            };
        }
    }
}
