using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	///
	/// </summary>
	public class UserSocialViewModel : ApiResult
	{
		/// <summary>
		/// 社会情况信息
		/// </summary>
		public UserSocialDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserSocialDataModel
	{
		/// <summary>
		/// 联系方式
		/// </summary>
		public string Phone { get; set; }

		/// <summary>
		///
		/// </summary>
		public Settle Settle { get; set; }

		/// <summary>
		/// 家庭地址
		/// </summary>
		public AdminDivision Address { get; set; }

		/// <summary>
		/// 家庭详细地址
		/// </summary>
		public string AddressDetail { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserSettleDataModel : Settle
	{
		/// <summary>
		///
		/// </summary>
		public new string Id { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public static class UserSocialExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static UserSocialDataModel ToDataModel(this UserSocialInfo model)
		{
			return new UserSocialDataModel()
			{
				Address = model.Address,
				AddressDetail = model.AddressDetail,
				Phone = model.Phone,
				Settle = model.Settle.ToModel()
			};
		}

		/// <summary>
		/// 格式化moment
		/// </summary>
		/// <param name="moment"></param>
		/// <returns></returns>
		public static Moment ToModel(this Moment moment)
		{
			return new Moment()
			{
				Address = new AdminDivision()
				{
					Code = moment?.Address?.Code ?? 0,
					Name = moment?.Address?.Name ?? "无",
					ParentCode = moment?.Address?.ParentCode ?? 0
				},
				AddressDetail = moment?.AddressDetail,
				Date = moment?.Date ?? DateTime.Now,
				Valid = moment?.Valid ?? false
			};
		}

		/// <summary>
		/// 格式化Settle
		/// </summary>
		/// <param name="settle"></param>
		/// <returns></returns>
		public static Settle ToModel(this Settle settle)
		{
			if (settle == null) settle = new Settle();
			return new Settle()
			{
				Lover = settle.Lover.ToModel(),
				Self = settle.Self.ToModel(),
				Parent = settle.Parent.ToModel(),
				LoversParent = settle.LoversParent.ToModel(),
				PrevYearlyLength = settle?.PrevYearlyLength ?? 0
			};
		}

		/// <summary>
		/// 格式化socialinfo
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static UserSocialInfo ToModel(this UserSocialDataModel model)
		{
			return new UserSocialInfo()
			{
				Address = new AdminDivision()
				{
					Code = model.Address?.Code ?? 0
				},
				AddressDetail = model.AddressDetail ?? "无",
				Phone = model.Phone,
				Settle = model.Settle.ToModel()
			};
		}
	}
}