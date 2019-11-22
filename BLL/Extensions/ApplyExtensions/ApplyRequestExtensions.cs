using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions.ApplyExtensions
{
	public static class ApplyRequestExtensions
	{

		public static int VocationTotalLength(this ApplyRequest model)
		{
			if (model?.StampReturn == null || !model.StampLeave.HasValue) return 0;
			return model.StampReturn.Value.Subtract(model.StampLeave.Value).Days;
		}
		public static string VocationDescription(this ApplyRequest model)
		{
			if (model == null) return "休假申请无效";
			return $"共计{model.OnTripLength + model.VocationLength}天(其中{model.VocationType}{model.VocationLength}天,路途{model.OnTripLength}天)";
		}
	}
}
