using BLL.Extensions.Common;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.Vacations;
using DAL.Entities.Vacations.VacationsStatistics;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services
{
	public class VacationStatisticsServices : IVacationStatisticsServices
	{
		private readonly ApplicationDbContext _context;

		public VacationStatisticsServices(ApplicationDbContext context)
		{
			_context = context;
		}

		public Tuple<IEnumerable<VacationStatisticsDescription>, int> Query(QueryVacationStatisticsViewModel model)
		{
			if (model == null) return null;
			var list = _context.VacationStatisticsDescriptions.AsQueryable();
			if (model.CompanyId?.Value != null) list = list.Where(v => v.Company.Code == model.CompanyId.Value);
			if (model.CompanyId?.Arrays != null) list = list.Where(v => model.CompanyId.Arrays.Contains(v.Company.Code));
			if (model.Id?.Value != null) list = list.Where(v => model.Id.Value == v.StatisticsId);
			if (model.Id?.Arrays != null) list = list.Where(v => model.Id.Arrays.Contains(v.StatisticsId));
			var result = list.SplitPage(model.Pages).Result;
			return new Tuple<IEnumerable<VacationStatisticsDescription>, int>(result.Item1.ToList(), result.Item2);
		}
	}
}