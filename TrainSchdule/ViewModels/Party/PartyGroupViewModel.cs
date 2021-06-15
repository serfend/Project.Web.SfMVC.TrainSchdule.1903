using DAL.DTO.ZZXT;
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
    public class PartyGroupViewModel:GoogleAuthViewModel 
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public PartyGroupDto Data { get; set; }
    }
}
