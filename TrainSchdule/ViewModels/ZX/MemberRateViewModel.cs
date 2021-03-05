using DAL.DTO.ZX.MemberRate;
using DAL.Entities.ZX.MemberRate;
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
        /// 评比单位，默认为用户所在单位
        /// </summary>
        public string CompanyCode { get; set; }
        /// <summary>
        /// 参评人
        /// </summary>
        public string UserId { get; set; }
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
                >= 400 => 500,
                _ => 100,
            };
            return new MemberRateDataModel()
            {
                CompanyCode = model.CompanyCode,
                Level = l,
                Rank = model.Rank,
                Remark = model.Remark,
                UserId = model.UserId
            };
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
            return new NormalRate() { 
                Company = companies.FirstOrDefault(i=>i.Code==model.Company),
                Create = DateTime.Now,
                Level = (int)model.Level,
                Rank=model.Rank,
                Remark=model.Remark,
                User = users.FirstOrDefault(i=>i.BaseInfo.Cid==model.UserCid)
            };
        }
    }
}
