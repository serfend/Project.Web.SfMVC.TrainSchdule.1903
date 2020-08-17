using Abp.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Company;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers
{
	/// <summary>
	/// 职务和单位类别等
	/// </summary>
	public partial class CompanyController
	{
		/// <summary>
		/// 获取指定岗位名称的详细信息
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult DutiesDetail(string name)
		{
			var currentUser = _currentUserService.CurrentUser;
			name = name ?? currentUser?.CompanyInfo?.Duties.Name;
			var duty = _context.Duties.Where(d => d.Name == name).FirstOrDefault();
			if (duty == null) return new JsonResult(ActionStatusMessage.CompanyMessage.DutyMessage.NotExist);
			var r = duty.ToDataModel();
			return new JsonResult(new DutyViewModel()
			{
				Data = r
			});
		}

		/// <summary>
		/// 获取指定名称可能的岗位
		/// </summary>
		/// <param name="name"></param>
		/// <param name="tag"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult DutiesQuery(string name, string tag, int pageIndex = 0, int pageSize = 20)
		{
			var dutiesQuery = _context.Duties.Where(d => d.Name != "NotSet");
			if (!name.IsNullOrEmpty()) dutiesQuery = dutiesQuery.Where(d => d.Name.Contains(name));
			if (!tag.IsNullOrEmpty()) dutiesQuery = dutiesQuery.Where(d => EF.Functions.Like(d.Tags, $"%{tag}%"));
			var result = dutiesQuery.SplitPage(new DAL.QueryModel.QueryByPage() { PageIndex = pageIndex, PageSize = pageSize });
			var data = new EntitiesListDataModel<DutyDataModel>()
			{
				List = result.Item1.Select(d => d.ToDataModel()),
				TotalCount = result.Item2
			};
			return new JsonResult(new DutiesViewModel()
			{
				Data = data
			});
		}

		/// <summary>
		/// 职务类别查询
		/// </summary>
		/// <param name="tag"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult DutiesTag(string tag, int pageIndex = 0, int pageSize = 20)
		{
			var pattern = $"%{tag}%";
			var dutiesQuery = _context.Duties.Where(d => d.Name != "NotSet");
			if (!tag.IsNullOrEmpty()) dutiesQuery = dutiesQuery.Where(d => EF.Functions.Like(d.Tags, pattern));
			var result = dutiesQuery.AsEnumerable()
				.SelectMany(d => d.Tags?.Split("##", StringSplitOptions.RemoveEmptyEntries) ?? new List<string>().ToArray())
				.Where(d => EF.Functions.Like(d, pattern))
				.Distinct()
				.OrderBy(a => a)
				.SplitPage(new DAL.QueryModel.QueryByPage() { PageIndex = pageIndex, PageSize = pageSize });
			return new JsonResult(new EntitiesListViewModel<string>(result.Item1, result.Item2));
		}

		/// <summary>
		/// 职务等级类别查询
		/// </summary>
		/// <param name="tag"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult DutiesTtileTag(string tag, int pageIndex = 0, int pageSize = 20)
		{
			var pattern = $"%{tag}%";
			var dutiesQuery = _context.UserCompanyTitles.Where(d => d.Name != "NotSet");
			if (!tag.IsNullOrEmpty()) dutiesQuery = dutiesQuery.Where(d => EF.Functions.Like(d.TitleType, pattern));
			var result = dutiesQuery.AsEnumerable()
				.SelectMany(d => d.TitleType?.Split("##", StringSplitOptions.RemoveEmptyEntries) ?? new List<string>().ToArray())
				.Where(d => EF.Functions.Like(d, pattern))
				.Distinct()
				.OrderBy(a => a)
				.SplitPage(new DAL.QueryModel.QueryByPage() { PageIndex = pageIndex, PageSize = pageSize });
			return new JsonResult(new EntitiesListViewModel<string>(result.Item1, result.Item2));
		}

		/// <summary>
		/// 检索可能的职务等级
		/// </summary>
		/// <param name="name"></param>
		/// <param name="tag"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult TitleQuery(string name, string tag, int pageIndex = 0, int pageSize = 20)
		{
			var dutiesQuery = _context.UserCompanyTitles.Where(d => d.Name != "NotSet");
			if (!name.IsNullOrEmpty()) dutiesQuery = dutiesQuery.Where(d => d.Name.Contains(name));
			if (!tag.IsNullOrEmpty()) dutiesQuery = dutiesQuery.Where(d => EF.Functions.Like(d.TitleType, $"%{tag}%"));
			var result = dutiesQuery.SplitPage(new DAL.QueryModel.QueryByPage() { PageIndex = pageIndex, PageSize = pageSize });
			var data = new EntitiesListDataModel<UserTitleDataModel>()
			{
				List = result.Item1.Select(d => d.ToDataModel()),
				TotalCount = result.Item2
			};
			return new JsonResult(new UserTitlesViewModel()
			{
				Data = data
			});
		}
	}
}