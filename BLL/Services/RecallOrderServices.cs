using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.Recall;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services
{
	public class RecallOrderServices : IRecallOrderServices
	{
		private readonly ApplicationDbContext _context;

		public RecallOrderServices(ApplicationDbContext context)
		{
			_context = context;
		}

		public RecallOrder Create(RecallOrderVDto recallOrder)
		{
			var order = new RecallOrder()
			{
				Create = DateTime.Now,
				ReturnStamp = recallOrder.ReturnStamp,
				Reason = recallOrder.Reason,
				HandleBy = _context.AppUsersDb.FirstOrDefault(u => u.Id == recallOrder.HandleBy.Id)
			};
			var apply = _context.AppliesDb.Where(a => a.Id == recallOrder.Apply).FirstOrDefault();
			if (apply == null) throw new ActionStatusMessageException(apply.NotExist());
			if (apply.RecallId != null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.RecallMessage.Crash);
			if (order.HandleBy == null) throw new ActionStatusMessageException(order.HandleBy.NotExist());
			if (!(apply.ApplyAllAuditStep.OrderBy(i=>i.Index).LastOrDefault()?.MembersAcceptToAudit.Split("##").Contains(recallOrder.HandleBy.Id) ?? false)) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.RecallMessage.RecallByNotSame);
			if (apply.RequestInfo.StampReturn <= order.ReturnStamp) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.RecallMessage.RecallTimeLateThanVacation);
			if (order.ReturnStamp < apply.RequestInfo.StampLeave) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.RecallMessage.RecallTimeEarlyThanVacationLeaveStamp);
			_context.RecallOrders.Add(order);
			apply.RecallId = order.Id;
			apply.ExecuteStatus |= ExecuteStatus.BeenSet;
			apply.ExecuteStatus |= ExecuteStatus.Recall;
			_context.Applies.Update(apply);
			_context.SaveChanges();
			return order;
		}

		public Tuple<ExecuteStatus,ApplyExecuteStatus> Create(IApplyRequestBase request, ExecuteStatus nowStatus, ExecuteStatusVDto status,bool canBeforeEndTime)
		{
			if (request == null || status == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.NotExist);
			var s = (int)nowStatus & (int)ExecuteStatus.BeenSet;
			if (s > 0) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.RecallMessage.Crash);
			nowStatus |= ExecuteStatus.BeenSet;
			var m = new ApplyExecuteStatus()
			{
				Create = DateTime.Now,
				HandleBy = _context.AppUsersDb.FirstOrDefault(u => u.Id == status.HandleBy.Id),
				ReturnStamp = status.ReturnStamp,
				Reason = status.Reason
			};
			var rawReturn = request.StampReturn?.Date;
			if (!canBeforeEndTime && m.ReturnStamp.Date < rawReturn) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.RecallMessage.SelfReturnNotPermit);
			else if (m.ReturnStamp.Date > rawReturn)
				nowStatus |= ExecuteStatus.Delay;
			_context.ApplyExcuteStatus.Add(m);
			return new Tuple<ExecuteStatus, ApplyExecuteStatus>(nowStatus,m);
		}
	}
}