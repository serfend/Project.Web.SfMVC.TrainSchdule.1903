using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ClientDevice
{
    public class ClientTagDto:BaseEntityGuid
    {
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 创建单位 用于限定权限
        /// </summary>
        public string CreateCompany { get; set; }
        public DateTime Create { get; set; }
        /// <summary>
        /// 已使用次数【缓存】
        /// </summary>
        public int Used { get; set; }
        /// <summary>
        /// 父标签
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}
