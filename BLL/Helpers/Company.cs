namespace BLL.Helpers
{
	public static partial class ActionStatusMessage
	{
		public static class Company
		{
			public static readonly ApiResult Default = new ApiResult(30000, "单位发生未知错误");
			public static readonly ApiResult NotExist = new ApiResult(31000, "单位不存在");
			public static readonly ApiResult NoneCompanyBelong = new ApiResult(32000, "用户不属于任何一个单位");
			public static readonly ApiResult CreateExisted = new ApiResult(33000, "创建的单位已经存在");
			public static class Manager
			{
				public static readonly ApiResult Default = new ApiResult(34000, "单位主管发生未知错误");
				public static readonly ApiResult NotExist=new ApiResult(34100, "单位中不存在此主管");
				public static readonly ApiResult Existed=new ApiResult(34200, "单位中已存在此主管");
			}
			public static class Duty
			{
				public static readonly ApiResult NotExist = new ApiResult(35100, "职务不存在");

			}
		}
	}
}
