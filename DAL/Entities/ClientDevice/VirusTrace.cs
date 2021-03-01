using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ClientDevice
{
    /// <summary>
    /// 溯源
    /// </summary>
    public class VirusTrace:BaseEntityGuid
    {
        /// <summary>
        /// 病毒类型 如为未识别的类型则按Sha1、(终端ip+时间出现(60秒)分类
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 仅病毒类型无法分类时使用
        /// </summary>
        public string Sha1 { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// 别称
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Description { get; set; }
    }
    public class VirusTypeDispatch : BaseEntityGuid
    {
        /// <summary>
        /// 病毒实体
        /// </summary>
        public virtual Virus Virus { get; set; }
        /// <summary>
        /// 所属类型
        /// </summary>
        public virtual VirusTrace VirusTrace { get; set; }
        /// <summary>
        /// 是否是自动分配
        /// </summary>
        public bool IsAutoDispatch { get; set; } 
    }
}
