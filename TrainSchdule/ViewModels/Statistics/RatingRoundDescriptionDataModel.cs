using DAL.Entities.ZX.MemberRate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Statistics
{
    /// <summary>
    /// 
    /// </summary>
    public class RatingRoundDescriptionDataModel
    {
        /// <summary>
        /// 轮数序数
        /// </summary>
        public int Round { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RoundToRoundIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RoundIndexToRound { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int NextRound { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int LastRound { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public RatingTypeDataModel Type { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class RatingTypeDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShortAlias { get; set; }
    }
}
