using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Entities.UserInfo;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	public class UserSocialViewModel:APIDataModel
	{
		public UserSocialDataModel Data { get; set; }
	}

	public class UserSocialDataModel
	{
		public string Phone { get; set; }
		public SettleDownEnum Settle { get; set; }
		public virtual AdminDivision Address { get; set; }
		public string AddressDetail { get; set; }
	}

	public static class UserSocialExtensions
	{
		public static UserSocialDataModel ToModel(this UserSocialInfo model)
		{
			return new UserSocialDataModel()
			{
				Address = model.Address,
				AddressDetail = model.AddressDetail,
				Phone = model.Phone,
				Settle = model.Settle
			};
		}
	}
}
