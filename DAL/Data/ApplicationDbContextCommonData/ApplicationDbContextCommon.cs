using DAL.Entities.Common.DataDictionary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public partial class ApplicationDbContext
    {

        public DbSet<CommonDataGroup> CommonDataGroups { get; set; }
        public DbSet<CommonDataDictionary> CommonDataDictionaries { get; set; }

		/// <summary>
		/// 数据字典 分组数据
		/// </summary>
		/// <param name="builder"></param>
		private void Configuration_Common_Group(ModelBuilder builder)
		{

			Configuration_Common_Applies(builder);
			Configuration_Common_Group_Client(builder);
			Configuration_Common_Group_Party(builder);
		}

		private void Configuration_Common(ModelBuilder builder)
		{
			var data = builder.Entity<CommonDataDictionary>();
			data.HasIndex(d => d.Key);

			Configuration_Common_Group(builder);
			Configuration_Statistics(builder);
			Configuration_Applies(builder);
			Configuration_Actions(builder);
			Configuration_Status(builder);
			Configuration_ExecuteStatus(builder);
			Configuration_ClientStatus(builder);
			Configuration_ZX(builder);
			Configuration_PartyTypeInParty(builder);
		}
	}
}
