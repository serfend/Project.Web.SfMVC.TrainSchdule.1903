using BLL.Interfaces.Common;
using DAL.Data;
using DAL.Entities.Common.DataDictionary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Common
{
    public class DataDictionariesServices : IDataDictionariesServices
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<CommonDataDictionary> db;

        public DataDictionariesServices(ApplicationDbContext context)
        {
            this.context = context;
            db = context.CommonDataDictionaries;
        }
        public IEnumerable<CommonDataDictionary> GetByGroupName(string groupName) => db.Where(d => d.GroupName == groupName);

    }
}
