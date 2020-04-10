using BLL.Helpers;
using BLL.Interfaces;
using DAL.Entities.UserInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	///
	/// </summary>
	public class UserCompanyInfoViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserCompanyInfoDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserCompanyDataModel
	{
		/// <summary>
		///
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		///
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///
		/// </summary>
		public string Parent { get; set; }

		/// <summary>
		///
		/// </summary>
		public IEnumerable<string> CompanyTags { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserCompanyInfoDataModel
	{
		/// <summary>
		///
		/// </summary>
		public UserCompanyDataModel Company { get; set; }

		/// <summary>
		///
		/// </summary>
		public string Duties { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserDutiesViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserDutiesDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserDutiesDataModel
	{
		/// <summary>
		/// 职务代码
		/// </summary>
		public int? Code { get; set; }

		/// <summary>
		///  职务名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 职务等级
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 职务生效时间
		/// </summary>
		public DateTime? TitleDate { get; set; }

		/// <summary>
		/// 是否主官
		/// </summary>
		public bool IsMajorManager { get; set; }

		/// <summary>
		/// 职务类别
		/// </summary>
		public IEnumerable<string> DutyTags { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public static class UserCompanyInfoExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <param name="companiesService"></param>
		/// <returns></returns>
		public static UserCompanyInfoDataModel ToCompanyModel(this UserCompanyInfo model, ICompaniesService companiesService)
		{
			return new UserCompanyInfoDataModel()
			{
				Company = new UserCompanyDataModel()
				{
					Code = model.Company?.Code,
					Name = model.Company?.Name,
					Parent = companiesService.FindParent(model.Company?.Code)?.Name,
					CompanyTags = model.Company?.Tag?.Split("##")
				},
				Duties = model.Duties?.Name
			};
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static UserDutiesDataModel ToDutiesModel(this UserCompanyInfo model)
		{
			return new UserDutiesDataModel()
			{
				Code = model.Duties?.Code,
				Name = model.Duties?.Name,
				Title = model.Title?.Name,
				TitleDate = model.TitleDate,
				IsMajorManager = model.Duties?.IsMajorManager ?? false,
				DutyTags = model.Duties?.Tags?.Split("##")
			};
		}
	}
}