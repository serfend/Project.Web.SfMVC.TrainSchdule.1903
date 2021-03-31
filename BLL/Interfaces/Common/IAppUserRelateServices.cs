using DAL.Entities.Common.Message;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Common
{
   public interface IAppUserRelateServices
    {
        /// <summary>
        /// 变更状态
        /// </summary>
        /// <param name="from"></param>
        /// <param name="target"></param>
        /// <param name="relation">变更状态</param>
        /// <param name="isAppend">添加/取消状态</param>
        /// <returns></returns>
        AppUserRelate Action(string from, string target, Relation relation,bool isAppend);
        /// <summary>
        /// 获取单个用户的有关系用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="direction">true:user=from;false:user=to</param>
        /// <returns></returns>
        IQueryable<AppUserRelate> RelateUser(string user,bool direction);

    }
}
