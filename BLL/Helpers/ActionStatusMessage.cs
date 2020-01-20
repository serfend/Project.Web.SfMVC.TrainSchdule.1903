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
		/// <summary>
		/// 在原报错信息基础上附加信息
		/// </summary>
		/// <param name="status"></param>
		/// <param name="message"></param>
		/// <param name="AppendToRawMessage"></param>
		public ApiResult(ApiResult status, string message, bool AppendToRawMessage)
		{
			this.status = status?.status??-25844;
			this.message = status?.message??"未定义类型";
			if (AppendToRawMessage) this.message += $" {AppendToRawMessage}";
		}
		public ApiResult() { }
	}
}
