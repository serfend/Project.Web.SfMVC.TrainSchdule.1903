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
			if (apply == null) throw new ActionStatusMessageException(ActionStatusMessage.Apply.NotExist);
			if (apply.RecallId != null) throw new ActionStatusMessageException(ActionStatusMessage.Apply.Recall.Crash);
			if (order.RecallBy == null) throw new ActionStatusMessageException(ActionStatusMessage.User.NotExist);
			if (apply.Response.LastOrDefault()?.AuditingBy.Id != order.RecallBy.Id) throw new ActionStatusMessageException(ActionStatusMessage.Apply.Recall.RecallByNotSame);
			if (apply.RequestInfo.StampReturn <= order.ReturnStramp) throw new ActionStatusMessageException(ActionStatusMessage.Apply.Recall.RecallTimeLateThanVocation);
			if (order.ReturnStramp <= DateTime.Now) throw new ActionStatusMessageException(ActionStatusMessage.Apply.Recall.RecallTimeEarlyThanNow);
			_context.RecallOrders.Add(order);
			apply.RecallId = order.Id;
			_context.Applies.Update(apply);
			_context.SaveChanges();
			return order;
		}
	}
}