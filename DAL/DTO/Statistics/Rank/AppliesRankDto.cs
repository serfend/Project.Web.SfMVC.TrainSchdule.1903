using DAL.Entities;
using DAL.Entities.Vacations.Statistics.Rank;
using DAL.Entities.ZX.MemberRate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Statistics.Rank
{
    public class AppliesRankDto:BaseEntityInt
    {
        public string ApplyType { get; set; }
        public string CompanyCode { get; set; }
        public string UserId { get; set; }
        public int RatingCycleCount { get; set; }
        public RatingType RatingType { get; set; }
        public int Rank { get; set; }
        public int Level { get; set; }
    }

    public static class ApplieRankExtensions
    {
        public static AppliesRankDto
            ToDto(this StatisticsApplyRank model)
        {
            return new AppliesRankDto()
            {
                ApplyType = model.ApplyType,
                CompanyCode = model.CompanyCode,
                Id = model.Id,
                Level = model.Level,
                Rank = model.Rank,
                RatingCycleCount = model.RatingCycleCount,
                RatingType = model.RatingType,
                UserId = model.UserId
            };
        }
    }
}
