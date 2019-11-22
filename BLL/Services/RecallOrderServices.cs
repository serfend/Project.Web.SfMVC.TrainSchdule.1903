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
		public RecallOrder Create( RecallOrderVDto recallOrder)
		{
			var order = new RecallOrder()
			{
				Apply=_context.Applies.Find(recallOrder?.Apply.Id),
				Create=DateTime.Now,
				ReturnStramp=recallOrder.ReturnStamp,
				Reason=recallOrder.Reason,
				RecallBy=_context.AppUsers.Find(recallOrder.RecallBy.Id)
			};
			if (order.Apply == null) throw new ActionStatusMessageException(ActionStatusMessage.Apply.NotExist);
			if (order.RecallBy == null) throw new ActionStatusMessageException(ActionStatusMessage.User.NotExist);
			if (order.Apply.Response.LastOrDefault()?.AuditingBy.Id != order.RecallBy.Id) throw new ActionStatusMessageException(ActionStatusMessage.Apply.Recall.RecallByNotSame);
			_context.RecallOrders.Add(order);
			order.Apply.RecallOrderId = order.Id;
			_context.SaveChanges();
			return order;
		}
	}
}
