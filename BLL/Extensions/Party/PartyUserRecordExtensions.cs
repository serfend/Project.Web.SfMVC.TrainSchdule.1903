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
                Create = model.Create,
                UserId = model.UserId
            };
        }

        public static PartyUserRecord ToModel(this PartyUserRecordDto model, ApplicationDbContext context, PartyUserRecord raw = null)
        {
            if (raw == null) raw = new PartyUserRecord();
            raw.Conference = context.PartyConferencesDb.FirstOrDefault(c => c.Id == model.Id);
            if (raw.Conference != null) raw.ConferenceId = model.ConferenceId;
            raw.Create = model.Create;
            raw.Id = model.Id;
            raw.IsRemoved = model.IsRemoved;
            raw.Type = model.Type;
            raw.User = context.AppUsersDb.FirstOrDefault(u => u.Id == model.UserId);
            if (raw.User != null) raw.UserId = model.UserId;
            return raw;
        }


        public static PartyConferRecordContentDto ToDto(this PartyUserRecordContent model)
        {
            return new PartyConferRecordContentDto()
            {
                Id = model.Id,
                Content = model.Content,
                ContentType = model.ContentType,
                IsRemovedDate = model.IsRemovedDate,
                Create = model.Create,
                IsRemoved = model.IsRemoved,
                RecordId = model.RecordId
            };
        }

        public static PartyUserRecordContent ToModel(this PartyConferRecordContentDto model, ApplicationDbContext context, PartyUserRecordContent raw = null)
        {
            if (raw == null) raw = new PartyUserRecordContent();
            raw.Record = context.PartyUserRecordsDb.FirstOrDefault(r => r.Id == model.RecordId);
            if (raw.Record != null) raw.RecordId = model.RecordId;
            raw.IsRemovedDate = model.IsRemovedDate;
            raw.ContentType = model.ContentType;
            raw.Content = model.Content;
            raw.Create = model.Create;
            raw.Id = model.Id;
            raw.IsRemoved = model.IsRemoved;
            return raw;
        }
    }
}
