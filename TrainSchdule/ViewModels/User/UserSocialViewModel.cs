using DAL.Entities;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using System.ComponentModel;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 
	/// </summary>
	public class UserSocialViewModel:ApiDataModel
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
		public  AdminDivision Address { get; set; }
		/// <summary>
		/// 家庭详细地址
		/// </summary>
		public string AddressDetail { get; set; }
	}
	public class UserSettleDataModel : Settle
	{
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
				Settle = model.Settle
			};
		}

		public static UserSocialInfo ToModel(this UserSocialDataModel model)
		{
			return new UserSocialInfo()
			{
				Address = model.Address,
				AddressDetail = model.AddressDetail,
				Phone = model.Phone,
				Settle = model.Settle
			};
		}
	}
}
