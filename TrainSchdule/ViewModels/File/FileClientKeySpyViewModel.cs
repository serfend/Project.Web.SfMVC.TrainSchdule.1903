using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.File
{
	/// <summary>
	/// 查询文件client-key
	/// </summary>
	public class FileClientKeySpyViewModel : GoogleAuthViewModel
	{
		/// <summary>
		/// 需查询的文件id
		/// </summary>
		public Guid Id { get; set; }
	}
}