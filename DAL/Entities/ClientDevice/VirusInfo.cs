using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ClientDevice
{
    public class Virus:BaseEntityGuid
    {
        /// <summary>
        /// 出现时间【冗余】
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// 状态变更时间【冗余】
        /// </summary>
        public DateTime HandleDate { get; set; }
        /// <summary>
        /// 病毒单一性描述
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 病毒路径
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 病毒类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Sha1
        /// </summary>
        public string Sha1 { get; set; }
        /// <summary>
        /// 处置状态
        /// </summary>
        public VirusStatus Status { get; set; }
        /// <summary>
        /// 来源终端
        /// </summary>
        public virtual Client Client { get; set; }
        /// <summary>
        /// 来源终端mid
        /// </summary>
        public string ClientMachineId { get; set; }
        /// <summary>
        /// 来源终端ip【冗余】
        /// </summary>
        public string ClientIp { get; set; }
        /// <summary>
        /// 责任人【冗余】
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// 责任单位【冗余】
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 病毒类型【冗余】
        /// </summary>
        public virtual VirusTrace TraceType { get; set; }
        /// <summary>
        /// 病毒类型名称【冗余】
        /// </summary>
        public string TraceAlias { get; set; }
    }
    public enum VirusStatus
    {
        [Description("无状态")]
        None=0,
        [Description("待处理")]
        Unhandle = 1,
        [Description("处置成功")]
        Success=2,
        [Description("终端推送已发出")]
        ClientNotify=4,
        [Description("第三方消息已发出")]
        MessageSend=8
    }
}
