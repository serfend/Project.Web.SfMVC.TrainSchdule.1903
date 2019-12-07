using BLL.Interfaces;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions.ApplyExtensions
{
	public static class ApplyRequestExtensions
	{
		
		public static int VocationTotalLength(this ApplyRequest model)
		{
			if (model?.StampReturn == null || !model.StampLeave.HasValue) return 0;
			return model.StampReturn.Value.Subtract(model.StampLeave.Value).Days+1;
		}
		public static string RequestInfoVocationDescription(this ApplyRequest model)
		{
			if (model == null) return "休假申请无效";
			var othersVocation = model.AdditialVocations.Sum(a => a.Length);
			var othersVocationDescription = new StringBuilder();
			foreach (var ov in model.AdditialVocations)othersVocationDescription.Append(ov.Name).Append(ov.Length).Append("天 ") ;
			return $"共计{model.VocationTotalLength()}天(其中{model.VocationType}{model.VocationLength}天,路途{model.OnTripLength}天 {othersVocationDescription.ToString()})";
		}
	}
}
