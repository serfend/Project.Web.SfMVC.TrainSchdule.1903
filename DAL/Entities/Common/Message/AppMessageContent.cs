using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Common.Message
{
    public class AppMessageContent:BaseEntityGuid
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
