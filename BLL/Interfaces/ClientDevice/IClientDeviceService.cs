using DAL.Entities.ClientDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.ClientDevice
{
    /// <summary>
    /// TODO 集中
    /// </summary>
    public interface IClientDeviceService
    {
        /// <summary>
        /// 更新病毒关联终端情况的缓存
        /// </summary>
        /// <param name="client"></param>
        public void UpdateClientRelate(Client client);
    }
}
