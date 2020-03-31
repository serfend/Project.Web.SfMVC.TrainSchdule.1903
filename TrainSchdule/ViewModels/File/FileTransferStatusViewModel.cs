using BLL.Helpers;
using DAL.Entities.FileEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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