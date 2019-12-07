namespace BLL.Helpers
{
	public static partial class ActionStatusMessage
	{
		public static class User
		{
			public static readonly ApiResult Default = new ApiResult(20000, "用户发生未知错误");
			public static readonly ApiResult NotExist = new ApiResult(21000, "用户不存在");
			public static readonly ApiResult NoCompany = new ApiResult(22000, "无归属单位");
			public static readonly ApiResult NotCorrectId=new ApiResult(23000, "非法的用户身份号，仅可为7位身份号或18位身份证号");
			public static readonly ApiResult NoId=new ApiResult(24000,"未填写用户身份号");


		}
	}
}
