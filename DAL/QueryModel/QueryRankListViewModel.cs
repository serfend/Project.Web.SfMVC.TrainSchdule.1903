using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.QueryModel
{
    public class QueryRankListViewModel
    {
        public QueryByPage Page { get; set; }
        /// <summary>
        /// 格式为 "申请类型.实体类型@统计类型"
        /// 如 inday.外出@l
        /// l:长度,c:数量
        /// </summary>
        public QueryByString EntityType { get; set; }
        public QueryByIntOrEnum RankType { get; set; }
        public QueryByString User { get; set; }
        public QueryByDate Date { get; set; }
        public QueryByString Company { get; set; }
    }
}
