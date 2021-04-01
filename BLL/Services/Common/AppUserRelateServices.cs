using BLL.Helpers;
using BLL.Interfaces.Common;
using DAL.Data;
using DAL.Entities.Common.Message;
using DAL.Entities.UserInfo;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Common
{
    public class AppUserRelateServices : IAppUserRelateServices
    {
        private readonly ApplicationDbContext context;
        private readonly IAppUserMessageInfoServices appUserMessageInfoServices;

        public AppUserRelateServices(ApplicationDbContext context, IAppUserMessageInfoServices appUserMessageInfoServices)
        {
            this.context = context;
            this.appUserMessageInfoServices = appUserMessageInfoServices;
        }
        public AppUserRelate Action(string from,string target, Relation relation, bool isAppend)
        {
            var item = context.AppUserRelates.ToExistDbSet().Where(r => r.FromId == from).Where(r => r.ToId == target).FirstOrDefault();
            if (item == null)
            {
                item = new AppUserRelate() {Create=DateTime.Now,FromId=from,ToId=target };
                context.Add(item);
            }
            var fromInfo = appUserMessageInfoServices.GetInfo(from);
            var toInfo = appUserMessageInfoServices.GetInfo(target);
            var current_realtion = item.Relation;
            switch ((relation,isAppend))
            {
                case (Relation.None,true):
                case (Relation.None, false):
                    return item;
                case (Relation.Follow,true):
                    if (current_realtion.HasFlag(Relation.Block))
                        current_realtion -= (int)Relation.Block;
                    if (!current_realtion.HasFlag(Relation.Follow))
                    {
                        BackgroundJob.Enqueue<AppMessageServices>(s => s.Send(target, from, "已关注", true));
                        fromInfo.FollowCount++;
                        toInfo.FansCount++;
                        context.UserAppMessageInfos.Update(fromInfo);
                        context.UserAppMessageInfos.Update(toInfo);
                        current_realtion += (int)Relation.Follow;
                    }
                    else throw new ActionStatusMessageException(ActionStatusMessage.AppMessage.AlreadyInStatus);
                    break;
                case (Relation.Follow, false):
                    if (current_realtion.HasFlag(Relation.Follow))
                    {
                        BackgroundJob.Enqueue<AppMessageServices>(s => s.Send(target, from, "已取消关注", true));
                        fromInfo.FollowCount--;
                        toInfo.FansCount--;
                        context.UserAppMessageInfos.Update(fromInfo);
                        context.UserAppMessageInfos.Update(toInfo);
                        current_realtion -= (int)Relation.Follow;
                    }
                    else throw new ActionStatusMessageException(ActionStatusMessage.AppMessage.AlreadyInStatus);
                    break;
                case (Relation.Block,true):
                    if (current_realtion.HasFlag(Relation.Follow))
                    {
                        current_realtion -= (int)Relation.Follow;
                        fromInfo.FollowCount--;
                        toInfo.FansCount--;
                        context.UserAppMessageInfos.Update(fromInfo);
                        context.UserAppMessageInfos.Update(toInfo);
                    }
                    if (!current_realtion.HasFlag(Relation.Block))
                    {
                        BackgroundJob.Enqueue<AppMessageServices>(s => s.Send(target, from, "已将对方加入到黑名单，将不再受到TA的消息", true));
                        current_realtion += (int)Relation.Block;
                    }
                    else throw new ActionStatusMessageException(ActionStatusMessage.AppMessage.AlreadyInStatus);
                    break;
                case (Relation.Block, false):
                    if (current_realtion.HasFlag(Relation.Block))
                    {
                        BackgroundJob.Enqueue<AppMessageServices>(s => s.Send(target, from, "已将对方从黑名单去除", true));
                        current_realtion -= (int)Relation.Block;
                    }
                    else throw new ActionStatusMessageException(ActionStatusMessage.AppMessage.AlreadyInStatus);
                    break;
                default:
                    break;
            }
            item.Relation = current_realtion;
            context.AppUserRelates.Update(item);
            context.SaveChanges(); // 保存两次，避免出现currency
            return item;
        }

        public IQueryable<AppUserRelate> RelateUser(string user, bool direction) => direction?context.AppUserRelates.Where(i => i.FromId == user):context.AppUserRelates.Where(i=>i.ToId==user);
    }
}
