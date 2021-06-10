using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Controllers.Party
{
    /// <summary>
    /// 测试api
    /// </summary>
    public class HealthTest:Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("health")]
        public IActionResult ApiTest()
        {
            return new JsonResult(new { success=true,status=0 });
        }
    }
}
