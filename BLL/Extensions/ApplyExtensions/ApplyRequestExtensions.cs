using DAL.Entities.ApplyInfo;
using System.Linq;
using System.Text;

namespace BLL.Extensions.ApplyExtensions
{
	public static class ApplyRequestExtensions
	{
		public static int VacationTotalLength(this ApplyRequest model)
		{
			if (model?.StampReturn == null || !model.StampLeave.HasValue) return 0;
			return model.StampReturn.Value.Subtract(model.StampLeave.Value).Days + 1;
		}

		public static string RequestInfoVacationDescription(this ApplyRequest model)
		{
			if (model == null) return "休假申请无效";
			var othersVacation = model.AdditialVacations.Sum(a => a.Length);
			var othersVacationDescription = new StringBuilder();
			foreach (var ov in model.AdditialVacations) othersVacationDescription.Append(ov.Name).Append(ov.Length).Append("天 ");
			return $"共计{model.VacationTotalLength()}天(其中{model.VacationType}{model.VacationLength}天,路途{model.OnTripLength}天 {othersVacationDescription.ToString()})";
		}
	}
}