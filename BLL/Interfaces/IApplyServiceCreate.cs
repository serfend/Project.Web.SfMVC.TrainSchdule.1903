using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;

namespace BLL.Interfaces
{
	public interface IApplyServiceCreate
	{
		Task<ApplyBaseInfo> SubmitBaseInfoAsync(ApplyBaseInfoDTO model);
		ApplyBaseInfo SubmitBaseInfo(ApplyBaseInfoDTO model);
		Task<ApplyRequest> SubmitRequestAsync(ApplyRequestDTO model);
		ApplyRequest SubmitRequest(ApplyRequestDTO model);

	}
}
