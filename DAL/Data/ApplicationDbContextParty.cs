using DAL.Entities.ZZXT;
using DAL.Entities.ZZXT.Conference;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    partial class ApplicationDbContext
    {
        public DbSet<UserPartyInfo> UserPartyInfos { get; set; }
        public IQueryable<UserPartyInfo> UserPartyInfosDb => UserPartyInfos.ToExistQueryable();
        public DbSet<PartyDuty> PartyDuties { get; set; }
        public IQueryable<PartyDuty> PartyDutiesDb => PartyDuties.ToExistQueryable();

        /// <summary>
        /// 党小组
        /// </summary>
        public DbSet<PartyGroup> PartyGroups { get; set; }
        public DbSet<PartyBaseConference> PartyConferences { get; set; }
        public IQueryable<PartyBaseConference> PartyConferencesDb => PartyConferences.ToExistQueryable();
        public DbSet<PartyConferWithTag> PartyConferWithTags { get; set; }
        /// <summary>
        /// 用户参会记录
        /// </summary>
        public DbSet<PartyUserRecord> PartyUserRecords { get; set; }
        public IQueryable<PartyUserRecord> PartyUserRecordsDb => PartyUserRecords.ToExistQueryable();

        /// <summary>
        /// 用户参会记录的内容
        /// </summary>
        public DbSet<PartyUserRecordContent> PartyUserRecordContents { get; set; }
        public IQueryable<PartyUserRecordContent> PartyUserRecordContentsDb => PartyUserRecordContents.ToExistQueryable();
        


    }
}
