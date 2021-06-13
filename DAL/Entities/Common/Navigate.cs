using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Common
{
    /// <summary>
    /// 导航
    /// </summary>
    public class CommonNavigate:BaseEntityGuid
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 使用远程图片
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 使用现有图标 Icon与此项应只设置一个
        /// </summary>
        public string Svg { get; set; }
        public string Url { get; set; }
    }

}
