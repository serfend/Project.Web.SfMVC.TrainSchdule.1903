using DAL.DTO.ClientDevice;
using DAL.Entities.ClientDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.ClientDevice
{
    public interface IClientVirusServices
    {
        void Edit(VirusDto model);
        /// <summary>
        /// 自动关联溯源
        /// </summary>
        /// <param name="client"></param>
        void RelateVirusTrace(Virus client);
        /// <summary>
        /// 手动关联溯源
        /// </summary>
        /// <param name="client"></param>
        /// <param name="trace"></param>
        void RelateVirusTrace(Virus client, VirusTrace trace);

    }
}
