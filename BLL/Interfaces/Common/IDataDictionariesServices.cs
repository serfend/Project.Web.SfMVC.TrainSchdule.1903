using DAL.Entities.Common.DataDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Common
{
    public interface IDataDictionariesServices
    {
        /// <summary>
        /// 获取指定分组的枚举
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        IEnumerable<CommonDataDictionary> GetByGroupName(string groupName);
    }
}
