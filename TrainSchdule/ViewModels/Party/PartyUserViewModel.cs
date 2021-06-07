using DAL.DTO.ZZXT;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Party
{
    /// <summary>
    /// 
    /// </summary>
    public class PartyUserViewModel : GoogleAuthViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public UserPartyInfoDto Data { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class QueryPartyUserViewModel
    {
        /// <summary>
        /// 单位代码
        /// </summary>
        public QueryByString Company { get; set; }
        /// <summary>
        /// 人员类别
        /// </summary>
        public QueryByIntOrEnum TypeInParty { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public QueryByIntOrEnum Labor { get; set; }
        /// <summary>
        /// 小组号
        /// </summary>
        public QueryByString Group { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public QueryByPage Page { get; set; }
    }
}
