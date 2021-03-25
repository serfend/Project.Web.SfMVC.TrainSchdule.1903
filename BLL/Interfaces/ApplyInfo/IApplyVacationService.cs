using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using DAL.Entities.ApplyInfo.DailyApply;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.ApplyInfo
{
    public interface IApplyVacationService:IApplyService<Apply,ApplyRequest>
    {
        ApplyRequestVdto CaculateVacation(ApplyRequestVdto model);
        ApplyRequest SubmitRequestAsync(User targetUser, ApplyRequestVdto model);
    }
    public interface IApplyInDayService : IApplyService<ApplyInday,ApplyIndayRequest>
    {
        ApplyIndayRequest SubmitRequestAsync(User targetUser, ApplyIndayRequestVdto model);

    }
}
