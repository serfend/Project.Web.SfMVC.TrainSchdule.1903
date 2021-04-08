using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Permisstions
{

    /// <summary>
    /// 角色
    /// </summary>
    public class PermissionsRole:IDbEntity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Key]
        public string Name { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual User CreateBy { get; set; }
        /// <summary>
        /// 创建人id
        /// </summary>
        public string CreateById { get; set; }
    }
}
