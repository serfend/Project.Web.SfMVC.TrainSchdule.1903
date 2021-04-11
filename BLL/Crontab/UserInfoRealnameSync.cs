using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.Crontab;

namespace BLL.Crontab
{
    public class UserInfoRealnameSync : ICrontabJob
    {
        private readonly ApplicationDbContext context;

        public UserInfoRealnameSync(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void Run()
        {
            var baseinfos = context.AppUserBaseInfos.Where(i=>i.PinYin==null).ToList();
            foreach(var u in baseinfos)
            {
                u.RealName = $"{u.RealName} ";
                u.RealName = u.RealName.Trim();
            }
            context.AppUserBaseInfos.UpdateRange(baseinfos);
            context.SaveChanges();
        }
    }
}
