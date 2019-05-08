using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Verify
{
	/// <summary>
	/// 
	/// </summary>
	public class ScrollerVerifyGeneratedViewModel:ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public ScrollerVerifyGeneratedDataModel Data { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class ScrollerVerifyGeneratedDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int PosY { get; set; }
	}
}
