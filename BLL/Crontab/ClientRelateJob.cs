using DAL.Data;
using DAL.Entities.ClientDevice;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.Crontab;

namespace BLL.Crontab
{
    public class ClientRelateJob : ICrontabJob
    {
        private readonly ApplicationDbContext context;

        public ClientRelateJob(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void RelateClientCompany()
        {
            var clients = context.ClientsDb
                .Where(c => c.CompanyCode == null)
                .Where(c => c.Ip != null)
                .Where(c => c.Ip != string.Empty)
                .Take(10).ToList();
            RelateProperty(clients, (prev, refererTo) =>
            {
                prev.CompanyCode = refererTo.CompanyCode;
            });
        }
        public void RelateClientMajor()
        {
            var clients = context.ClientsDb
                .Where(c => c.OwnerId == null)
                .Where(c => c.CompanyCode != null)
                .Where(c => c.Ip != null)
                .Where(c => c.Ip != string.Empty)
                .Take(10).ToList();
            var companies = clients
                .Select(c => c.CompanyCode)
                .Distinct()
                .Select(c =>
                {
                    var managers = context.AppUsersDb
                        .Where(u => u.CompanyInfo.CompanyCode == c)
                        .Where(u => u.CompanyInfo.Duties.IsMajorManager);
                    return new KeyValuePair<string,User>(c, managers.FirstOrDefault());
                }).ToDictionary(c=>c.Key);
            foreach(var c in clients)
                c.Owner = companies[c.CompanyCode].Value;
        }
        private void RelateProperty(IEnumerable<Client> clients, Action<Client, Client> callback)
        {
            foreach (var c in clients)
            {
                var ips = c.IpInt >> 8;
                var ipin = c.IpInt & 0xff;
                var sameRegion = context.ClientsDb
                    .Where(client => client.CompanyCode != null)
                    .Where(client => client.Ip != null)
                    .Where(client => client.IpInt % 256 == ips)
                    .ToList();
                var nearest = sameRegion.OrderBy(x=> Math.Abs(ipin - (x.IpInt >> 8))).FirstOrDefault();
                if (nearest == null) continue;
                callback.Invoke(c, nearest);
            }
        }
        public void Run()
        {
            RelateClientCompany();
            RelateClientMajor();
            context.SaveChanges();
        }
    }
}
