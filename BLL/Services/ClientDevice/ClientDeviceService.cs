using BLL.Interfaces.ClientDevice;
using DAL.Data;
using DAL.Entities.ClientDevice;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ClientDevice
{
    public class ClientDeviceService : IClientDeviceService
    {
        private readonly ApplicationDbContext context;

        public ClientDeviceService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void UpdateClientRelate(Client client)
        {
            // -------------------TODO 实现缓存和限流 本方法 -------------------
            const string UpdateClientRelateKey = "UpdateClientRelate";
            var _cache = new MemoryCache(new MemoryCacheOptions()
            {
                ExpirationScanFrequency = TimeSpan.FromMinutes(30)
            });
            var cache = _cache.Get<DateTime?>(UpdateClientRelateKey);
            if (cache == null || cache > DateTime.Now) return;
            _cache.Set<DateTime?>(UpdateClientRelateKey,DateTime.Now.AddSeconds(10));
            // -------------------TODO 实现缓存和限流 -------------------

            var company = client.CompanyCode;
            var owner = client.OwnerId;
            var virues = context.VirusesDb.Where(v => v.ClientMachineId == client.MachineId).ToList();
            virues.ForEach(v => {
                v.Company = company;
                v.Owner = owner;
            });
            context.Viruses.UpdateRange(virues);
            context.SaveChanges();
        }
    }
}
