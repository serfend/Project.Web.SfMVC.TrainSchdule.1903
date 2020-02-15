using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services.Game_r3.R3ResultContent
{
	public class ApiResult
	{
		[JsonProperty("msg")]
		public string Message { get; set; }
		public string Code { get; set; }
	}
	public class GiftCodeResult : ApiResult
	{

	}
	public class SimpleUserInfoResult : ApiResult
	{
		public RawUserInfo Data { get; set; }
	}
	public class RawUserInfo
	{
		[JsonProperty("name")]
		public string NickName { get; set; }
		[JsonProperty("title")]
		public string Level { get; set; }
	}
}
