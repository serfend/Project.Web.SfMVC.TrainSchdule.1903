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
        public IQueryable<UserPartyInfo> UserPartyInfosDb => UserPartyInfos.ToExistDbSet();
        public DbSet<PartyDuty> PartyDuties { get; set; }
        public IQueryable<PartyDuty> PartyDutiesDb => PartyDuties.ToExistDbSet();

        /// <summary>
        /// 党小组
        /// </summary>
        public DbSet<PartyGroup> PartyGroups { get; set; }
        public DbSet<PartyConference> PartyConferences { get; set; }
        public DbSet<PartyConferWithTag> PartyConferWithTags { get; set; }
        /// <summary>
        /// 用户参会记录
        /// </summary>
        public DbSet<PartyUserRecord> PartyUserRecords { get; set; }

    }
}
