using DAL.DTO.ClientDevice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.ClientDevice
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientWithTagViewModel : GoogleAuthViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public ClientWithTagDataModel Data { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ClientWithTagDataModel 
    {
        /// <summary>
        /// 终端id
        /// </summary>
        [Required(ErrorMessage = "终端id未填写")]
        public string MachineId { get; set; }
        /// <summary>
        /// 包含的标签
        /// </summary>
        [Required(ErrorMessage = "标签列表未填写")]
        public IEnumerable<string> Tags { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ClientTagDataModel : GoogleAuthViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage ="标签内容未填写")]
        public ClientTagDto Data { get; set; }
        /// <summary>
        /// 是否允许覆盖
        /// </summary>
        public bool AllowOverwrite { get; set; }
    }

}
