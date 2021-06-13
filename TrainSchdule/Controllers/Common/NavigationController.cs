using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Controllers.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NavigationController {
    }
    /// <summary>
    /// 菜单管理
    /// </summary>

    [Route("[controller]/[action]")]
    [ApiController]
    public partial class NavigationController:Controller
    {
        private readonly ApplicationDbContext context;
        /// <summary>
        /// 
        /// </summary>
        public NavigationController(ApplicationDbContext context)
        {
            this.context = context;
        }
    }
}
