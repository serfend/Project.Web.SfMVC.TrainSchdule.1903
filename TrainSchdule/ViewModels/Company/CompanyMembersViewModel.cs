using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.DAL.Entities.UserInfo;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Company
{
	public class CompanyMembersViewModel:APIDataModel
	{
		public CompanyMembersDataModel Data { get; set; }
	}

	public class CompanyMembersDataModel
	{
		public IEnumerable<CompanySingleMemberDataModel> List { get; set; }
	}

	public class CompanySingleMemberDataModel
	{
		public string UserName { get; set; }
		public string RealName { get; set; }
		public GenderEnum Gender { get; set; }
		/// <summary>
		/// 单位名称
		/// </summary>
		public string Company { get; set; }
		public string Duties { get; set; }
	}
}
