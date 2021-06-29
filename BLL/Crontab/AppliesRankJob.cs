using BLL.Interfaces.IVacationStatistics.Rank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.Crontab;

namespace BLL.Crontab
{
    public class AppliesRankJob : ICrontabJob
    {
        private readonly IStatisticsApplyRankServices statisticsApplyRankServices;

        public AppliesRankJob(IStatisticsApplyRankServices statisticsApplyRankServices)
        {
            this.statisticsApplyRankServices = statisticsApplyRankServices;
        }
        public void Run()
        {
            statisticsApplyRankServices.Reload();
        }
    }
    public class AppliesRankReloadYearJob : ICrontabJob
    {
        private readonly IStatisticsApplyRankServices statisticsApplyRankServices;

        public AppliesRankReloadYearJob(IStatisticsApplyRankServices statisticsApplyRankServices)
        {
            this.statisticsApplyRankServices = statisticsApplyRankServices;
        }
        public void Run()
        {
            var now = DateTime.Now;
            var d = new DateTime(now.Year, 1, 1);
            statisticsApplyRankServices.ReloadRange(d, now);
        }
    }
}
