using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ClientDevice
{
    public class Client : BaseEntityGuid
    {
        /// <summary>
        /// mid
        /// </summary>
        public string MachineId { get; set; }
        /// <summary>
        /// 终端ip
        /// </summary>
        public string Ip{ get; set; }
        /// <summary>
        /// 终端ip值
        /// </summary>
        public int IpInt { get; set; }
        /// <summary>
        /// 终端mac
        /// </summary>
        public string Mac { get; set; }
        /// <summary>
        /// 类型型号
        /// </summary>
        public string DeviceType { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public virtual UserInfo.User Owner { get; set; }
        /// <summary>
        /// 负责人姓名【冗余】
        /// </summary>
        public virtual string OwnerId { get; set; }
        /// <summary>
        /// 所属单位
        /// </summary>
        public virtual Company Company { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string FutherInfo { get; set; }
    }
}
