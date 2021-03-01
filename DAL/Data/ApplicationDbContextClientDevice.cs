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
        public IQueryable<Client> ClientsDb => Clients.ToExistDbSet();
        public DbSet<Virus> Viruses { get; set; }
        public IQueryable<Virus> VirusesDb => Viruses.ToExistDbSet();
        public DbSet<VirusHandleRecord> VirusHandleRecords { get; set; }
        public IQueryable<VirusHandleRecord> VirusHandleRecordsDb => VirusHandleRecords.ToExistDbSet();
        public DbSet<VirusTrace> VirusTraces { get; set; }
        public IQueryable<VirusTrace> VirusTracesDb => VirusTraces.ToExistDbSet();
        public DbSet<VirusTypeDispatch> VirusTypeDispatches { get; set; }
        public IQueryable<VirusTypeDispatch> VirusTypeDispatchesDb => VirusTypeDispatches.ToExistDbSet();
    }
}
