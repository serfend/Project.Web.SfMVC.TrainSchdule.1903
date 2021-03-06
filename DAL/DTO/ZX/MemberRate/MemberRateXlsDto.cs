using DAL.Entities.ZX.MemberRate;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ZX.MemberRate
{
    public class MemberRateXlsDto
    {
        /// <summary>
        /// 周期数:当前评分模式下距离 Date(0) 
        /// </summary>
        [Required(ErrorMessage = "周期数未设置")]
        public int RatingCycleCount { get; set; }
        /// <summary>
        /// 评分模式
        /// </summary>
        [Required(ErrorMessage = "评分模式未设置")]
        public RatingType RatingType { get; set; }
        /// <summary>
        /// 评比单位
        /// </summary>
        [Required(ErrorMessage = "评比单位")]
        public string Company { get; set; }
        /// <summary>
        /// xls数据文件
        /// </summary>
        [Required(ErrorMessage = "数据文件未上传")]
        public IFormFile File { get; set; }
        /// <summary>
        /// 是否覆盖原记录
        /// </summary>
        public bool Confirm { get; set; }
    }
}
