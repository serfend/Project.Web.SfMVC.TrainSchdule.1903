using Abp.Linq.Expressions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Common;
using DAL.Data;
using DAL.Entities.Common.Message;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.UserAppMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Common
{
    public class AppMessageServices : IAppMessageServices
    {
        private readonly ApplicationDbContext context;
        private readonly IAppUserMessageInfoServices appUserMessageInfoServices;
        private readonly IAppUserRelateServices appUserRelateServices;
        private readonly ICurrentUserService currentUserService;

        public AppMessageServices(ApplicationDbContext context, IAppUserMessageInfoServices appUserMessageInfoServices, IAppUserRelateServices appUserRelateServices, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.appUserMessageInfoServices = appUserMessageInfoServices;
            this.appUserRelateServices = appUserRelateServices;
            this.currentUserService = currentUserService;
        }
        public AppMessage Action(string user,Guid message, Interfaces.Common.Action action)
        {
            var msg = context.BBSMessages.FirstOrDefault(i =>i.Id==message) ?? throw new ActionStatusMessageException(ActionStatusMessage.AppMessage.LoadFail);
            switch (action)
            {
                case Interfaces.Common.Action.CheckExist:
                    return context.BBSMessages.FirstOrDefault(i=>i.Id==message);
                case Interfaces.Common.Action.Recall:
                    if (user != msg.FromId) throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.Default);
                    if (!msg.Status.HasFlag(AppMessageStatus.Recall))
                    {
                        if(DateTime.Now.Subtract(msg.Create).TotalSeconds>120) throw new ActionStatusMessageException(ActionStatusMessage.AppMessage.RecallToLate);
                        msg.Status |= AppMessageStatus.Recall;
                        context.BBSMessages.Update(msg);
                    }
                    break;
                case Interfaces.Common.Action.Delete:
                    if (user != msg.FromId && user!= msg.ToId)
                        throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.Default);
                    bool status_attach = false;
                    if (user == msg.FromId)
                    {
                        status_attach = !msg.Status.HasFlag(AppMessageStatus.DeletedByFrom);
                        if (status_attach)
                            msg.Status |= AppMessageStatus.DeletedByFrom;
                    }else if (user == msg.ToId)
                    {
                        status_attach = !msg.Status.HasFlag(AppMessageStatus.DeletedByTo);
                        if (status_attach)
                            msg.Status |= AppMessageStatus.DeletedByTo;
                    }
                    context.BBSMessages.Update(msg);
                    break;
                default:
                    break;
            }
            context.SaveChanges();
            return msg;
        }

        public IQueryable<AppMessage> GetUnread(string user) => context.BBSMessages
                .Where(m => !m.Status.HasFlag(AppMessageStatus.Read))
                .Where(m => m.ToId == user);
        public IEnumerable<AppMessage> GetDetail(string from,string to)
        {
            var currentUser = currentUserService.CurrentUser;
            if (currentUser.Id != from && currentUser.Id != to) throw new ActionStatusMessageException(ActionStatusMessage.AppMessage.LoadFail);
            var msgs = context.BBSMessages.Where(m => !m.Status.HasFlag(AppMessageStatus.Read)).Where(i => i.FromId == from).Where(i=>i.ToId==to).ToList();
            foreach(var msg in msgs)
            {
                if (!msg.Status.HasFlag(AppMessageStatus.Recall) && !msg.Status.HasFlag(AppMessageStatus.Read))
                    msg.Status |= AppMessageStatus.Read;
            }
            context.BBSMessages.UpdateRange(msgs);
            context.SaveChanges();
            return msgs;
        }
        public Tuple<IQueryable<AppMessage>, int> Query(MessageQueryItem item)
        {
            var list = context.BBSMessages.ToExistDbSet();
            var create = item.Create;
            if(create.Start!=DateTime.MinValue && create.End != DateTime.MinValue)
                list = list.Where(i => i.Create >= create.Start).Where(i => i.Create <= create.End);
            var from = item.From?.Value;
            if (from == null)
                from = currentUserService.CurrentUser.Id;
            list = list.Where(i =>i.FromId==from);
            var to = item.To?.Value;
            if (to != null)
                list = list.Where(i => i.ToId == to);
            var content = item.Content?.Value;
            if (content != null)
                list = list.Where(i => i.Status.HasFlag(AppMessageStatus.Read)).Where(i => i.Content.Content.Contains(content));
            var result = list.SplitPage(item.Pages);
            return result;
        }

        public AppMessage Send(string from,string to, string content,bool isSystem)
        {
            if (!isSystem)
            {
                var relate_by = appUserRelateServices.RelateUser(from, false).FirstOrDefault(u => u.FromId == to)?.Relation ?? Relation.None;
                if (relate_by.HasFlag(Relation.Block)) throw new ActionStatusMessageException(ActionStatusMessage.AppMessage.MessageRejectByBlock);
                var config = appUserMessageInfoServices.GetInfo(to);
                if (!relate_by.HasFlag(Relation.Follow) && !config.Setting.HasFlag(AppMessageSetting.AllowStrangerMessage)) throw new ActionStatusMessageException(ActionStatusMessage.AppMessage.MessageRejectByNotFriend);
            }
            var message = new AppMessage() { 
                FromId = from,
                ToId = to,
                Content = new AppMessageContent()
                {
                    Content = content
                },
                Length = content.Length,
                Create= DateTime.Now,
                Status= isSystem?AppMessageStatus.IsSystem:AppMessageStatus.None
            };
            context.BBSMessages.Add(message);
            context.SaveChanges();
            message = context.BBSMessages.FirstOrDefault(i => i.Id == message.Id);
            return message;
        }
    }
}
