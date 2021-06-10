using DAL.Entities.ZZXT.Conference;
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
    public class PartyConferenceViewModel : GoogleAuthViewModel, ICommonDataUpdate
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "会议信息未填写")]
        public PartyBaseConference Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool AllowOverwrite { get; set; }
    }

}
