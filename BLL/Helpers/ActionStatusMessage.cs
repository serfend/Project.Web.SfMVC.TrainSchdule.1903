namespace BLL.Helpers
{
	public static partial class ActionStatusMessage
	{
		public static readonly ApiResult Success = new ApiResult(0, null);
		public static readonly ApiResult Fail = new ApiResult(-1, "失败");
	}

	public class ApiResult
	{
		private int status;
		private string message;

		public int Status { get => status; set => status = value; }
		public string Message { get => message; set => message = value; }

		public ApiResult(int status, string message)
		{
			this.Status = status;
			this.Message = message;
		}
	}
}
