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
    public class ConferWithTagViewModel : GoogleAuthViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public ConferWithTagDataModel Data { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ConferWithTagDataModel
    {
        /// <summary>
        /// 终端id
        /// </summary>
        [Required(ErrorMessage = "会议id未填写")]
        public string Id { get; set; }
        /// <summary>
        /// 包含的标签
        /// </summary>
        [Required(ErrorMessage = "标签列表未填写")]
        public IEnumerable<string> Tags { get; set; }
    }
}
