using BLL.Extensions;
using DAL.DTO.User;
using DAL.DTO.ZX.MemberRate;
using DAL.Entities.ZX.MemberRate;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.ZX
{
    /// <summary>
    /// 评比
    /// </summary>
    public class MemberRateDataModel
    {
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }
        /// <summary>
        /// 分数 0 - 1000，可映射到等级 0-200不称职 201-400较差 401-600称职 601-800良好 801-1000优秀
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 用户基本信息
        /// </summary>
        public UserSummaryDto User { get; set; }
        /// <summary>
        /// 周期数:当前评分模式下距离 Date(0) 
        /// </summary>
        public int RatingCycleCount { get; set; }
        /// <summary>
        /// 评分模式
        /// </summary>
        public RatingType RatingType { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class MemberRateQueryModel
    {
        /// <summary>
        /// 评比单位
        /// </summary>
        public QueryByString Company { get; set; }
        /// <summary>
        /// 被评比人
        /// </summary>
        public QueryByString User { get; set; }
        /// <summary>
        /// 周期数:当前评分模式下距离 Date(0) 
        /// </summary>
        public QueryByIntOrEnum RatingCycleCount { get; set; }
        /// <summary>
        /// 评分模式
        /// </summary>
        public QueryByIntOrEnum RatingType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public QueryByPage Page { get; set; }

    }
    /// <summary>
    /// 
    /// </summary>
    public static class MemberRateExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static MemberRateDataModel ToDataModel(this NormalRate model)
        {
            int l =
            ((int)model.Level) switch
            {
                >= 800 => 900,
                >=600 => 700,
                >= 400 => 500,
                >= 200 => 300,
                _ => 100,
            };
            var t = new MemberRateDataModel()
            {
                User=model.User.ToSummaryDto(),
                //CompanyCode = model.CompanyCode,
                Level = l,
                Rank = model.Rank,
                Remark = model.Remark,
                //UserId = model.UserId,
                RatingCycleCount = model.RatingCycleCount,
                RatingType = model.RatingType
            };
            //var user = model.User;
            //if (user != null)
            //{
            //    var b = user.BaseInfo;
            //    t.RealName = b.RealName;
            //    t.Cid = b.Cid;
            //    var c = user.CompanyInfo;
            //    t.CompanyName = c.Company.Name;
            //    t.Duty = c.Duties.Name;
            //}
            return t;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="companies"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public static NormalRate ToModel(this MemberRateImportDto model,IQueryable<DAL.Entities.Company> companies,IQueryable<DAL.Entities.UserInfo.User>users)
        {
            var raw = new NormalRate()
            {
                Company = companies.FirstOrDefault(i => i.Code == model.Company),
                Create = DateTime.Now,
                Level = (int)model.Level,
                Rank = model.Rank,
                Remark = model.Remark,
                User = users.FirstOrDefault(i => i.BaseInfo.Cid == model.UserCid),
            };
            return raw;
        }
    }
}
