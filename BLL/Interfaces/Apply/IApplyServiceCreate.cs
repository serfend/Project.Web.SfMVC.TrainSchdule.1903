using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;

namespace BLL.Interfaces
{
	public interface IApplyServiceCreate
	{
		Task<ApplyBaseInfo> SubmitBaseInfoAsync(ApplyBaseInfoVdto model);

		ApplyRequest SubmitRequestAsync(User targetUser, ApplyRequestVdto model);

		ApplyRequestVdto CaculateVacation(ApplyRequestVdto model);

		Apply Submit(ApplyVdto model);
		IQueryable<Apply> CheckIfHaveSameRangeVacation(Apply apply);
	}
}