using BLL.Extensions;
using BLL.Interfaces;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.Crontab;

namespace BLL.Crontab
{
    public class UserOrderJob : ICrontabJob
    {
        private readonly ApplicationDbContext context;

        public UserOrderJob(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// 重新排序用户顺序
        /// </summary>
        public void Run()
        {
            OrderCompanies();
            OrderUsers();
            context.SaveChanges();
        }
        private void OrderCompanies()
        {
            var companies = context.CompaniesDb.ToList();
            var sum_dict = new Dictionary<string, uint>();
            int now_length = 1; // 当前单位层级
            while (true)
            {
                var level_companies = companies.Where(c => c.Code.Length == now_length).ToList();
                if (level_companies.Count == 0) break;
				foreach(var c in level_companies)
                {
                    var last_level_code = c.Code.Substring(0, now_length - 1);
                    var prev_level_sum = sum_dict.ContainsKey(last_level_code)?sum_dict[last_level_code]:0;
                    var cur_level_sum = (prev_level_sum << 1) + c.Priority;
                    c.PrioritySum = cur_level_sum;
                    sum_dict[c.Code] = cur_level_sum;
                }
                now_length++;
            }
            context.Companies.UpdateRange(companies);
        }
        private void OrderUsers()
        {
            var users = context.AppUsersDb.OrderByCompanyAndTitle().ToList();
            long index = 0;
            foreach (var u in users)
                u.UserOrderRank = index--;
            context.AppUsers.UpdateRange(users);
        }
    }
}
