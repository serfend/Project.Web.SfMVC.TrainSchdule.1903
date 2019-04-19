using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Account
{
	public class UserInfoViewModel:APIDataModel
	{
		public UserInfoDataModel Data { get; set; }
	}

	public class UserInfoDataModel
	{
		[JsonProperty("avatar")]
		public string Avatar { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("introduction")]
		public string Description { get; set; }
		[JsonProperty("roles")]
		public IEnumerable<string> Rules { get; set; }
	}
}
