using DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
	public partial class UsersService
	{
		public IEnumerable<Company> InMyManage(string id)
		{
			var list = _context.CompanyManagers.Where(m => m.User.Id == id).Select(m=>m.Company);
			return list;
		}
	}
}
