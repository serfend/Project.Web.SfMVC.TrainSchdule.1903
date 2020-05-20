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
				ReturnStramp = recallOrder.ReturnStamp,
				Reason = recallOrder.Reason,
				RecallBy = _context.AppUsers.Find(recallOrder.RecallBy.Id)
			};
			var apply = _context.AppliesDb.Where(a => a.Id == recallOrder.Apply).FirstOrDefault();
			if (apply == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.NotExist);
			if (apply.RecallId != null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Recall.Crash);
			if (order.RecallBy == null) throw new ActionStatusMessageException(ActionStatusMessage.UserMessage.NotExist);
			if (!(apply.ApplyAllAuditStep.LastOrDefault()?.MembersAcceptToAudit.Split("##").Contains(order.RecallBy.Id) ?? false)) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Recall.RecallByNotSame);
			if (apply.RequestInfo.StampReturn <= order.ReturnStramp) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Recall.RecallTimeLateThanVacation);
			if (order.ReturnStramp <= DateTime.Now) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Recall.RecallTimeEarlyThanNow);
			_context.RecallOrders.Add(order);
			apply.RecallId = order.Id;
			_context.Applies.Update(apply);
			_context.SaveChanges();
			return order;
		}
	}
}