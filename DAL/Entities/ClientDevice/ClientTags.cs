using DAL.DTO.ClientDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.ClientDevice
{
    public class ClientTags:BaseEntityGuid
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
        public virtual ClientTags Parent { get; set; }
        public Guid? ParentId { get; set; }
    }

    public static class ClientTagExtensions
    {
        public static ClientTagDto ToDto(this ClientTags c)
        {
            if (c == null) return null;
            return new ClientTagDto()
            {
                 Create=c.Create,
                 Description=c.Description,
                 CreateCompany=c.CreateCompany,
                 Id=c.Id,
                 IsRemoved=c.IsRemoved,
                 IsRemovedDate=c.IsRemovedDate,
                 Name=c.Name,
                 ParentId=c.ParentId,
                 Used=c.Used
            };
        }
        public static ClientTags ToModel(this ClientTagDto c)
        {
            return new ClientTags()
            {
                Create = c.Create,
                Description = c.Description,
                CreateCompany = c.CreateCompany,
                Id = c.Id,
                IsRemoved = c.IsRemoved,
                IsRemovedDate = c.IsRemovedDate,
                Name = c.Name,
                ParentId = c.ParentId,
                Used = c.Used
            };
        }
    }
}
