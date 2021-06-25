using Abp.Extensions;
using DAL.Data;
using DAL.Entities.ClientDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.Crontab;

namespace BLL.Crontab
{
    public class VirusRelateClientJob : ICrontabJob
    {
        private readonly ApplicationDbContext context;

        public VirusRelateClientJob(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void RelateClientInfo(DateTime date)
        {
            var virusDb = context.VirusesDb;
            // 未能标记信息的终端项
            var unRelateToClientsVirus = virusDb
                .Where(v => v.Client != null)
                .Where(v => v.Owner == null || v.Company == null)
                .Where(v => v.Create >= date)
                .ToList();

            var clientDb = context.ClientsDb;
            var clientDictCache = new Dictionary<string, Client>();
            // 标记这些项
            unRelateToClientsVirus.ForEach(v =>
            {
                var client = v.Client;
                if (client != null)
                {
                    v.Company = client.CompanyCode;
                    v.Owner = client.OwnerId;
                    context.Viruses.Update(v);
                }
            });
        }
        public void RelateClient(DateTime date)
        {
            var virusDb = context.VirusesDb;
            // 未能标记终端的项
            var unRelateToClientsVirus = virusDb
                .Where(v => v.Client == null)
                .Where(v => v.ClientIp != null)
                .Where(v => v.Create >= date)
                .ToList();

            var clientDb = context.ClientsDb;
            var clientDictCache = new Dictionary<string, Client>();
            // 标记这些项
            unRelateToClientsVirus.ForEach(v =>
            {
                if (!clientDictCache.ContainsKey(v.ClientIp)) clientDictCache[v.ClientIp] = clientDb.FirstOrDefault(c => c.Ip == v.ClientIp);
                var client = clientDictCache[v.ClientIp];
                if (client != null)
                {
                    v.Client = client;
                    v.ClientMachineId = client.MachineId;
                    v.Company = client.CompanyCode;
                    v.Owner = client.OwnerId;
                    context.Viruses.Update(v);
                }
            });
        }
        public void Run()
        {
            var date = DateTime.Now.Subtract(TimeSpan.FromDays(7));
            RelateClientInfo(date);
            RelateClient(date);
            context.SaveChanges();
        }
    }
}
