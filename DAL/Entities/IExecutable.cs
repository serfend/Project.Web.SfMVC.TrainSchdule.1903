using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public interface IExecutable
	{
		/// <summary>
		/// 休假落实状态，需要联动修改<see cref="ExecuteStatus"/>
		/// </summary>

		[ForeignKey("ExecuteStatusDetailId")]
        public  ApplyExecuteStatus ExecuteStatusDetail { get; set; }
		/// <summary>
		/// 休假落实状态
		/// </summary>
		public ExecuteStatus ExecuteStatus { get; set; }

		public Guid? ExecuteStatusDetailId { get; set; }
	}
}
