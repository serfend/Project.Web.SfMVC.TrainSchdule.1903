using DAL.DTO.ClientDevice;
using DAL.Entities.ClientDevice;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.ClientDevice
{
    /// <summary>
    /// 
    /// </summary>
    public class VirusQueryDataModel
    {
        /// <summary>
        /// 病毒的id
        /// </summary>
        public QueryByGuid Id { get; set; }
        /// <summary>
        /// 创建人用户名
        /// </summary>
        public QueryByString CreateBy { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public QueryByString Companies { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public QueryByDate Create { get; set; }
        /// <summary>
        /// <see cref="VirusStatus"/>
        /// </summary>
        public QueryByIntOrEnum Status { get; set; }
        /// <summary>
        /// 来源终端的mid
        /// </summary>
        public QueryByString Client { get; set; }
        /// <summary>
        /// 来源终端的ip
        /// </summary>
        public QueryByString Ip { get; set; }
        /// <summary>
        /// 病毒名
        /// </summary>
        public QueryByString FileName { get; set; }
        /// <summary>
        /// 病毒类型
        /// </summary>
        public QueryByString Type { get; set; }

        /// <summary>
        /// 分页
        /// </summary>
        public QueryByPage Pages { get; set; }
    }
}
