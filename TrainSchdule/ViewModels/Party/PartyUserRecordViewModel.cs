using DAL.Entities.ZZXT;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Party
{
    /// <summary>
    /// 
    /// </summary>
    public class PartyUserRecordViewModel : GoogleAuthViewModel, ICommonDataUpdate
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "用户记录未填写")]
        public PartyUserRecord Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool AllowOverwrite { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PartyUserRecordDataModel { }
}
