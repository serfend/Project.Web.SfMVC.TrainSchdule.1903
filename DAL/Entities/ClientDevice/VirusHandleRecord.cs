using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ClientDevice
{
    /// <summary>
    /// 病毒处置记录
    /// </summary>
    public class VirusHandleRecord : BaseEntityGuid
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// 所属对象
        /// </summary>
        public virtual Virus Virus { get; set; }
        /// <summary>
        /// 操作状态
        /// </summary>
        public VirusHandleStatus HandleStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }
        /// <summary>
        /// 所属病毒Key【冗余】
        /// </summary>
        public string VirusKey { get; set; }
        /// <summary>
        /// 所属终端mid【冗余】
        /// </summary>
        public string ClientMachineId { get; set; }
    }
    public enum VirusHandleStatus
    {
        [Description("无")]
        None =0,
        [Description("终端设备")]
        ClientDevice = 256,
        [Description("终端设备病毒")]
        ClientDeviceVirus = 384,
        [Description("发出染毒通告")]
        ClientDeviceVirusNotify = 388,
        [Description("发出染毒即时消息")]
        ClientDeviceVirusMessage = 389,
        [Description("终端设备新增待处置")]
        ClientDeviceVirusNew = 416,
        [Description("终端设备新增处置成功")]
        ClientDeviceVirusNewSuccess = 417,
        [Description("终端设备新增未处置")]
        ClientDeviceVirusNewUnhandle = 418,
        [Description("终端设备新增处置失败")]
        ClientDeviceVirusNewFail = 419,
        [Description("终端设备新增已处置")]
        ClientDeviceVirusHandle = 448,
        [Description("自主处置")]
        ClientDeviceVirusHandleByUser = 449,
        [Description("通过第三方处置")]
        ClientDeviceVirusHandleByIgnore = 461,
        [Description("通过提交方式处置")]
        ClientDeviceVirusHandleBySubmit = 481,
        [Description("其他")]
        Other = 512
    }
    public static class VirusHandleStatusExtension
    {
        public static bool IsSuccess(this VirusHandleStatus model) => (int)model >= ((int)VirusHandleStatus.ClientDeviceVirusHandle) && (int)model < (int)VirusHandleStatus.Other;
    }
}
