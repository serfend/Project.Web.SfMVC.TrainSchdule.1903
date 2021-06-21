using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.Extensions.Common.EntityModifyExtensions;

namespace BLL.Interfaces.Common
{
    public interface IDataUpdateServices
    {
        /// <summary>
        /// 通用更新实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        (ActionType,T) Update<T>(DataUpdateModel<T> model)where T:BaseEntityGuid;
    }
}
