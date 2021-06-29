using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.Statistics.Rank;
using DAL.Entities.Vacations.Statistics.Rank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions.Statistics
{

    public static class RankListExtensions
    {
        public static AppliesRankListDto ToDto(this Tuple<IEnumerable<StatisticsApplyRankItem>, StatisticsApplyRankItem> model, int totalCount)
        {
            var list = model.Item1;
            var item = model.Item2;
            return new AppliesRankListDto()
            {
                Self = new RankItem()
                {
                    Rank = item.Rank,
                    Status = item.Status,
                    LastRank = item.LastRank,
                    RealName = item.User.BaseInfo.RealName,
                    User = item.UserId,
                    Level = item.Level
                },
                TotalCount = totalCount,
                List = list.Select(i => new RankItem()
                {
                    Rank = i.Rank,
                    LastRank = i.LastRank,
                    Status = i.Status,
                    RealName = i.UserRealName,
                    User = i.UserId,
                    Level = i.Level,
                })
            };
        }
    }
}
