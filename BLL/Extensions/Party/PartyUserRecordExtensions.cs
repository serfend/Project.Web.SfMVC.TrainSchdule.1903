using DAL.DTO.ZZXT;
using DAL.Entities.ZZXT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions.Party
{
    public static class PartyUserRecordExtensions
    {
        public static PartyUserRecordDto ToDto(this PartyUserRecord model)
        {
            return new PartyUserRecordDto()
            {
                IsRemovedDate = model.IsRemovedDate,
                ConferenceId = model.ConferenceId,
                Id = model.Id,
                IsRemoved = model.IsRemoved,
                Type = model.Type,
                UserId = model.UserId
            };
        }
    }
}
