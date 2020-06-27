namespace BLL.Helpers
{
	public static partial class ActionStatusMessage
	{
		public static class Grade
		{
			public static class Subject
			{
				public static readonly ApiResult NotExist = new ApiResult(61100, "无此科目");
				public static readonly ApiResult AlreadyExisted = new ApiResult(61110, "已存在此科目");
			}

			public static class Compute
			{
				public static readonly ApiResult UnExpected = new ApiResult(61200, "计算错误");
			}
		}
	}
}