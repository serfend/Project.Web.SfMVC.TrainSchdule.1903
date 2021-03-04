using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.Common.Message;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.BBS
{
    /// <summary>
	/// 站内消息
	/// </summary>
	[Route("[controller]/[action]")]
    [Authorize]
    public partial class BBSMessageController:Controller
    {
        private readonly ApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="currentUserService"></param>
        public BBSMessageController(ApplicationDbContext context,ICurrentUserService currentUserService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
        }
    }


    public partial class BBSMessageController
    {
        /// <summary>
        /// 近期消息目录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Summary() {
            var user = currentUserService.CurrentUser;
            var userid = user.Id;
            var messagesGet = context.BBSMessagesDb.Where(i => i.ToId == userid);
            var messagesSend = context.BBSMessagesDb.Where(i => i.FromId == userid);
            return new JsonResult(ActionStatusMessage.Success);
        }
    }
}
