using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Entities.ApplyInfo;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService
	{
		public void RemoveAllUnSaveApply()
		{
			var list = _context.Applies
				.Where(a => a.Status == AuditStatus.NotSave)
				.Where(a=>a.Create.HasValue && a.Create.Value.AddDays(1).CompareTo(DateTime.Now)<0).ToList();
			if (list.Count == 0) return;
			foreach (var apply in list)
			{
				_context.ApplyResponses.RemoveRange(apply.Response);
			}
			_context.Applies.RemoveRange(list);
			foreach (var apply in list)
			{
				_context.ApplyBaseInfos.Remove(apply.BaseInfo);
				_context.ApplyRequests.Remove(apply.RequestInfo);
			}

			var applies = _context.Applies;
			var request = _context.ApplyRequests.Where(r => !applies.Any(a => a.RequestInfo.Id == r.Id)).Where(r=>DateTime.Now.Day!= r.CreateTime.Day);
			_context.ApplyRequests.RemoveRange(request);
			_context.SaveChanges();
		}
	}
}
