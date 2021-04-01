using DAL.Entities.UserInfo.UserAppMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Common
{
    public interface IAppUserMessageInfoServices
    {
        UserAppMessageInfo GetInfo(string user);
    }
}
