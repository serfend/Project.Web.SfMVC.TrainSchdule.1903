using DAL.Data;
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

        public VirusRelateClientJob(ApplicationDbContext context) {
            this.context = context;
        }
        public void Run()
        {
            var virusDb = context.VirusesDb;
            // 未能标记终端的项
            var unRelateToClientsVirus = virusDb.Where(v => v.Client == null).Where(v=>v.ClientIp!=null).ToList();

            var clientDb = context.ClientsDb;
            // 标记这些项
            unRelateToClientsVirus.ForEach(v =>
            {
                var client = clientDb.FirstOrDefault(c => c.Ip == v.ClientIp);
                if (client != null)
                {
                    v.Client = client;
                    v.ClientMachineId = client.MachineId;
                    context.Viruses.Update(v);
                }
            });
            context.SaveChanges();
        }
    }
}
