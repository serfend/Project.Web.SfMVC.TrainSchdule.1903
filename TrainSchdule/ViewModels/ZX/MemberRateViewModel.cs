using BLL.Extensions;
using DAL.DTO.User;
using DAL.DTO.ZX.MemberRate;
using DAL.Entities.ZX.MemberRate;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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
        /// 等级名称
        /// </summary>
        public string LevelName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 用户基本信息
        /// </summary>
        public UserSummaryDto User { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 评比单位代码
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// 评比单位名称
        /// </summary>
        public string CompanyName { get; set; }
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
        public static List<Tuple<int, string>> ValueToLevelAssign { get; }
        static MemberRateExtensions(){
            var type = typeof(LevelAssign);
            ValueToLevelAssign = new List<Tuple<int, string>>();
            foreach (LevelAssign s in Enum.GetValues(type))
            {
                MemberInfo[] mi = type.GetMember(s.ToString());
                if (mi != null && mi.Length > 0 && Attribute.GetCustomAttribute(mi[0], typeof(DisplayAttribute)) is DisplayAttribute attr)
                    ValueToLevelAssign.Add(new Tuple<int, string>((int)s,attr.Name));
            }
            ValueToLevelAssign.Sort((a,b)=>b.Item1-a.Item1);
        }
        /// <summary>
        /// 值转换为等级名称
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToLevelName(this int v) => ValueToLevelAssign.FirstOrDefault(l => l.Item1 <= v)?.Item2;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static MemberRateDataModel ToDataModel(this NormalRate model)
        {
            
            var t = new MemberRateDataModel()
            {
                User=model.User.ToSummaryDto(),
                CompanyCode = model.CompanyCode,
                CompanyName = model.Company?.Name,
                Level = model.Level,
                LevelName = ToLevelName(model.Level),
                Rank = model.Rank,
                Remark = model.Remark,
                UserId = model.UserId,
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
