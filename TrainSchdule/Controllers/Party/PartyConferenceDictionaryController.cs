using DAL.Data;
using DAL.Entities.Common.DataDictionary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.Party
{

    /// <summary>
    /// 会议字典获取
    /// </summary>
    public partial class PartyConferenceController
    {
        /// <summary>
        /// 获取会议类型字典
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ConferTypeDict()
        {
            return new JsonResult(new EntitiesListViewModel<CommonDataDictionary>(dataDictionariesServices.GetByGroupName(ApplicationDbContext.PartyConferType)));
        }
        /// <summary>
        /// 获取会议记录内容类型字典
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ConferContentTypeDict()
        {
            return new JsonResult(new EntitiesListViewModel<CommonDataDictionary>(dataDictionariesServices.GetByGroupName(ApplicationDbContext.PartyConferRecordType)));
        }
    }
}
