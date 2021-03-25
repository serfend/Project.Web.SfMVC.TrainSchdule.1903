using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.ApplyInfo
{
    public interface IApplyServiceClear
    {

		/// <summary>
		/// 删除指定时间内无用户认领的申请
		/// </summary>
		/// <param name="interval"></param>
		/// <returns></returns>
		Task<int> RemoveAllNoneFromUserApply(TimeSpan interval);

	}
}
