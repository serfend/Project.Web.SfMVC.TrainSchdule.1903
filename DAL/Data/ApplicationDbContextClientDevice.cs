using DAL.Entities.ClientDevice;
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
        public DbSet<Client> Clients { get; set; }
        public IQueryable<Client> ClientsDb => Clients.Where(i => !i.IsRemoved);
        public DbSet<Virus> Viruses { get; set; }
        public IQueryable<Virus> VirusesDb => Viruses.Where(i => !i.IsRemoved);
        public DbSet<VirusHandleRecord> VirusHandleRecords { get; set; }
        public IQueryable<VirusHandleRecord> VirusHandleRecordsDb => VirusHandleRecords.Where(i => !i.IsRemoved);
    }
}
