namespace BLL.Helpers
{
	public static partial class ActionStatusMessage
	{
		public static class Statistics
		{
			public static readonly ApiResult NotExist = new ApiResult(71100, "无此id的统计数据");
			public static readonly ApiResult AllreadyExist = new ApiResult(71200, "此id的统计数据已存在");
			
		}
	}
}
