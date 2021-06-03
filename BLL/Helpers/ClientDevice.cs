using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Helpers
{
    public static partial class ActionStatusMessage
    {
        public static class ClientDevice { 
            public static class Client
            {
                public static readonly ApiResult Exist = new ApiResult(111110, "终端已存在");
                public static readonly ApiResult NotExist = new ApiResult(111120, "终端不存在");
            }
        }
    }
}
