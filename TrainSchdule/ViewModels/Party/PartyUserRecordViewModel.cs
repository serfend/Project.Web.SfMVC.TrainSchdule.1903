using DAL.DTO.ZZXT;
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
    public class PartyConferRecordContentViewModel : GoogleAuthViewModel, ICommonDataUpdate
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "记录内容未填写")]
        public PartyConferRecordContentDto Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool AllowOverwrite { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class PartyUserRecordViewModel : GoogleAuthViewModel, ICommonDataUpdate
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "用户记录未填写")]
        public PartyUserRecordDto Data { get; set; }
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
