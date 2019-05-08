namespace TrainSchdule.ViewModels.Verify
{
	/// <summary>
	/// 谷歌授权码
	/// </summary>
	public class GoogleAuthViewModel
	{
		/// <summary>
		/// 授权权限来源
		/// </summary>
		public string AuthByUserID { get; set; }
		/// <summary>
		/// 授权码
		/// </summary>
		public int Code { get; set; }
	}
}
