using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Extensions;
using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using ExcelReport;
using ExcelReport.Driver.NPOI;
using ExcelReport.Renderers;

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
			_context.SaveChanges();
			var applies = _context.Applies;
			var request = _context.ApplyRequests.Where(r => !applies.Any(a => a.RequestInfo.Id == r.Id)).Where(r => DateTime.Now.Day != r.CreateTime.Day);
			_context.ApplyRequests.RemoveRange(request);
			var baseInfos = _context.ApplyBaseInfos.Where(r => !applies.Any(a => a.BaseInfo.Id == r.Id)).Where(r => DateTime.Now.Day != r.CreateTime.Day);
			_context.ApplyBaseInfos.RemoveRange(baseInfos);
			_context.SaveChanges();
		}

		public byte[] ExportExcel(string templete, string outPath, ApplyDetailDto model)
		{
			var sheetRenderers=new SheetRenderer[]
			{
				new SheetRenderer("Sheet1",
					new ParameterRenderer("RealName", model.Base?.RealName),//真实姓名不允许修改
					new ParameterRenderer("CompanyName", model.Company?.Name),
					new ParameterRenderer("StampLeave", model.RequestInfo?.StampLeave),
					new ParameterRenderer("StampReturn", model.RequestInfo?.StampReturn),
					new ParameterRenderer("HomeAddressDetail", model.Social?.AddressDetail),
					new ParameterRenderer("Phone", model.Social?.Phone),
					new ParameterRenderer("VocationType",model.RequestInfo?.VocationType),
					new ParameterRenderer("VocationDescription",model.RequestInfo.VocationDescription()),
					new ParameterRenderer("ByTransportation",model.RequestInfo?.ByTransportation),
					new ParameterRenderer("VocationLength",model.RequestInfo.VocationTotalLength()))
			};
			return Export.ExportToBuffer(templete, sheetRenderers);
		}

		public byte[] ExportExcel(string templete, string outPath, IEnumerable<ApplyDetailDto> model)
		{
			throw new NotImplementedException();
		}
	}
}
