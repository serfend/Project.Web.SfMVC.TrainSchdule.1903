using System.Collections.Generic;
using DAL.Entities;

namespace BLL.Interfaces
{
	public interface IUserServiceDetail
	{
		/// <summary>
		/// 用户为主管的单位
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		IEnumerable<Company> InMyManage(string id);
	}
}
