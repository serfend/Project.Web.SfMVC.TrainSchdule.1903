using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Helpers
{
    public static partial class ActionStatusMessage
    {
        public class AppMessage
        {
            public static readonly ApiResult Default = new ApiResult(101000, "未知的站内消息错误");
            public static readonly ApiResult LoadFail = new ApiResult(101100, "消息加载失败");
            public static readonly ApiResult MessageRejectByBlock = new ApiResult(101200, "消息已发出，但被对方拒收");
            public static readonly ApiResult MessageRejectByNotFriend = new ApiResult(101300, "对方拒收Ta没有关注的用户发来的消息");
            public static readonly ApiResult AlreadyInStatus = new ApiResult(101400, "状态已变更过，不需要重复操作~");

            
        }
    }
}
