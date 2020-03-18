using BLL.Helpers;
using DAL.Entities.FileEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.File
{
	public class FileTransferStatusViewModel : ApiResult
	{
		public UploadCache Data { get; set; }
	}
}