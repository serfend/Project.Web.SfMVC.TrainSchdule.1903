using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.BLL.DTO.UserInfo;

namespace TrainSchdule.Web.ViewModels.Company
{
	public class GetDicViewModel:APIDataModel
	{
		public GetDicDataModel Data { get; set; }
	}

	public class GetDicDataModel
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public IEnumerable<CompanyDTO> Child { get; set; }
	}
}
