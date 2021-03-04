using DAL.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Controllers.Zx
{
    /// <summary>
    /// 成员评分
    /// </summary>
    public partial class MemberRateController:Controller
    {
        private readonly ApplicationDbContext context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public MemberRateController(ApplicationDbContext context)
        {
            this.context = context;
        }
    }
    public partial class MemberRateController
    {

    }
}
