using BLL.Helpers;
using DAL.Entities.FileEngine;

namespace TrainSchdule.ViewModels.File
{
	/// <summary>
	///
	/// </summary>
	public class FileTransferStatusViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UploadCache Data { get; set; }
	}
}