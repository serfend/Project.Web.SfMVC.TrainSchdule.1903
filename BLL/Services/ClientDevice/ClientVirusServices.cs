using Abp.Extensions;
using BLL.Helpers;
using BLL.Interfaces.ClientDevice;
using DAL.Data;
using DAL.DTO.ClientDevice;
using DAL.Entities.ClientDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ClientDevice
{
    public class ClientVirusServices : IClientVirusServices
    {
        private readonly ApplicationDbContext context;

        public ClientVirusServices(ApplicationDbContext
            context)
        {
            this.context = context;
        }
        public void RelateVirusTrace(Virus client)
        {
            VirusTrace trace = null;
            if (client.Type?.ToLower()?.Equals("trojan.generic") ?? false)
                trace = context.VirusTracesDb.FirstOrDefault(i => i.Sha1 == client.Sha1);
            else
                trace = context.VirusTracesDb.FirstOrDefault(i => i.Type == client.Type);
            if (trace == null)
            {
                trace = new VirusTrace() { Type = client.Type, Sha1 = client.Sha1, Create = DateTime.Now };
                context.VirusTraces.Add(trace);
            }
            var dispatch = context.VirusTypeDispatchesDb.FirstOrDefault(i=>i.Virus.Id==client.Id);
            if (dispatch != null && !dispatch.IsAutoDispatch) return; // 当用户手动修改过后不再自动同步
            if (dispatch == null) dispatch = new VirusTypeDispatch() { IsAutoDispatch = true, Virus = client, VirusTrace = trace };
            else dispatch.VirusTrace = trace;
            context.VirusTypeDispatches.Add(dispatch);
            SynVirusTrace(client, trace);
        }

        public void RelateVirusTrace(Virus client, VirusTrace trace)
        {
            var relate = context.VirusTypeDispatchesDb.FirstOrDefault(i => i.Virus.Id == client.Id);
            if (relate != null)
            {
                if (relate.VirusTrace.Id == trace.Id) throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.ResourceAllReadyExist);
                relate.VirusTrace = trace;
                relate.IsAutoDispatch = false;
                context.VirusTypeDispatches.Update(relate);
            }
            else
                context.VirusTypeDispatches.Add(new VirusTypeDispatch() { IsAutoDispatch = false, Virus = client, VirusTrace = trace });
            SynVirusTrace(client, trace);
        }
        public void SynVirusTrace(Virus client,VirusTrace trace)
        {
            client.TraceType = trace;
            client.TraceAlias = trace.Alias;
            context.Viruses.Update(client);
        }
        public void Edit(VirusDto model)
        {
            if (model.Key.IsNullOrEmpty()) throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.IdIsNull);
            var r = context.VirusesDb.FirstOrDefault(i => i.Key == model.Key);
            var client = r ?? new Virus() { Status = VirusStatus.Unhandle };
            model.ToModel(context.Clients, client);
            if (client.IsRemoved && r != null)
            {
                context.Viruses.Update(r);
                r.Remove();
            }
            if (r == null)
            {
                RelateVirusTrace(client);
                context.Viruses.Add(client);
            }
            else context.Viruses.Update(client);
        }
    }
}
