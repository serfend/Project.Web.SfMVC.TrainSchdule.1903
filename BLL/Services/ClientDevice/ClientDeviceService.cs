using BLL.Interfaces.ClientDevice;
using DAL.Data;
using DAL.Entities.ClientDevice;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis.Extensions.Core.Abstractions;
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
        private readonly IRedisCacheClient redisCacheClient;

        public ClientDeviceService(ApplicationDbContext context, IRedisCacheClient redisCacheClient)
        {
            this.context = context;
            this.redisCacheClient = redisCacheClient;
        }
        public void UpdateClientRelate(Guid client)
        {
            const string UpdateClientRelateKey = "com:job:client:relate";
            var cache = redisCacheClient.Db7.ExistsAsync(UpdateClientRelateKey).Result;
            if (cache) return;
            redisCacheClient.Db7.AddAsync(UpdateClientRelateKey, DateTime.Now, TimeSpan.FromSeconds(10));

            var clientItem = context.ClientsDb.FirstOrDefault(c => c.Id == client);
            if (clientItem == null) return;
            var company = clientItem.CompanyCode;
            var owner = clientItem.OwnerId;
            var virues = context.VirusesDb
                .Where(v => v.ClientMachineId == clientItem.MachineId)
                .Where(v => v.Company != company || v.Owner != owner)
                .ToList();
            virues.ForEach(v =>
            {
                v.Company = company;
                v.Owner = owner;
            });
            context.Viruses.UpdateRange(virues);
            context.SaveChanges();
        }

        public void UpdateClientTags(Guid prevClient, Guid client)
        {
            const string UpdateClientRelateKey = "com:job:client:relate-tag";
            var cache = redisCacheClient.Db7.ExistsAsync(UpdateClientRelateKey).Result;
            if (cache) return;
            redisCacheClient.Db7.AddAsync(UpdateClientRelateKey, DateTime.Now, TimeSpan.FromSeconds(10));


            var clientItem = context.ClientsDb.FirstOrDefault(c => c.Id == client);
            if (clientItem == null) return;

            var prevClientItem = context.ClientsDb.FirstOrDefault(c => c.Id == prevClient);
            if (prevClientItem == null) return;
            var prevTags = context.ClientWithTags
                .Where(c => c.ClientId == prevClient)
                .Select(t => t.ClientTags)
                .ToList();
            prevTags.ForEach(t =>
            {
                t.Used--;
            });
            context.ClientTags.UpdateRange(prevTags);
            var tags = context.ClientWithTags
                .Where(c => c.ClientId == client)
                .Select(t => t.ClientTags)
                .ToList();
            tags.ForEach(t =>
            {
                t.Used++;
            });
            context.ClientTags.UpdateRange(tags);
            context.SaveChanges();
        }
    }
}
