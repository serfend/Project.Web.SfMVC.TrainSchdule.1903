using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Common;
using DAL.Data;
using DAL.Entities.UserInfo.UserAppMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Common
{
    public class AppUserMessageInfoServices : IAppUserMessageInfoServices
    {
        private readonly ApplicationDbContext context;
        private readonly IUsersService usersService;

        public AppUserMessageInfoServices(ApplicationDbContext context,IUsersService usersService)
        {
            this.context = context;
            this.usersService = usersService;
        }
        public UserAppMessageInfo GetInfo(string user)
        {
            var u = usersService.GetById(user);
            if (u == null) throw new ActionStatusMessageException(u.NotExist());
            var item = context.UserAppMessageInfos.FirstOrDefault(u => u.UserId == user);
            if (item == null)
            {
                item = new UserAppMessageInfo()
                {
                    Setting = AppMessageSetting.AllowAddByCompany | AppMessageSetting.AllowAddByGroup | AppMessageSetting.AllowAddByScan | AppMessageSetting.AllowAddByShareCard | AppMessageSetting.AllowAddByUserId | AppMessageSetting.AllowStrangerMessage,
                    UserId = user
                };
                context.UserAppMessageInfos.Add(item);
                context.SaveChanges();
            }
            return context.UserAppMessageInfos.FirstOrDefault(u => u.UserId == user);
        }
    }
}
