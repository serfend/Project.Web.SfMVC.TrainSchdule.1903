using DAL.Entities.UserInfo;
using DAL.Entities.ZX.MemberRate;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ZX.MemberRate
{
    [ExcelImporter(IsLabelingError =true)]
    public class MemberRateImportDto
    {

        /// <summary>
        /// 参评人
        /// </summary>
        [ImporterHeader(Name = "身份证号",IsAllowRepeat = false,IsInterValidation =true)]
        [Required(ErrorMessage = "身份证号未输入")]
        [IDCard]
        public string UserCid { get; set; }

        /// <summary>
        /// 评比单位，默认为用户所在单位
        /// </summary>
        [ImporterHeader(Name = "评比单位", Description = "默认为用户所在单位，如需更改则输入单位代码", IsInterValidation = true)]
       
        public string Company { get; set; }
        /// <summary>
        /// 排名
        /// </summary>
        [ImporterHeader(Name ="排名",IsAllowRepeat =false,Description ="本单位内排序", IsInterValidation = true)]
        [Required(ErrorMessage ="排名未选择")]
        public int Rank { get; set; }
        /// <summary>
        /// 分数 0 - 1000，可映射到等级 0-200不称职 201-400较差 401-600称职 601-800良好 801-1000优秀
        /// </summary>
        [ImporterHeader(Name = "等次", IsInterValidation = true)]
        [Required(ErrorMessage ="等次未选择")]
        public LevelAssign Level { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [ImporterHeader(Name = "备注", IsInterValidation = true)]
        public string Remark { get; set; }
    }
    public enum LevelAssign
    {
        [Display(Name ="无", AutoGenerateField = false)]
        None = 0,
        [Display(Name ="不称职")]
        L1 = 100,
        [Display(Name ="较差", AutoGenerateField = false)]
        L2 = 300,
        [Display(Name ="称职")]
        L3 = 500,
        [Display(Name ="良好",AutoGenerateField =false)]
        L4 = 700,
        [Display(Name ="优秀")]
        L5 = 900,
    }

}
