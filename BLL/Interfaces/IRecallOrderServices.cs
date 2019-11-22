using DAL.DTO.Recall;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
	public interface IRecallOrderServices
	{
		RecallOrder Create(RecallOrderVDto recallOrder);
	}
}
