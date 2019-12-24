using DAL.DTO.User;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
	/// <summary>
	/// 用户服务接口.
	/// 包含用户信息的相关处理.
	/// </summary>
	/// <remarks>
	/// 此接口需要反射调用.
	/// </remarks>
	public interface IUsersService :  IUserServiceDetail
	{
		string ConvertFromUserCiper(string username, string password);
		/// <summary>
		/// 加载所有用户的信息
		/// </summary>
		IEnumerable<User> GetAll(int page, int pageSize);

        User Get(string Id);

        IQueryable<User> Find(Expression<Func<User, bool>>predict);

        ApplicationUser ApplicaitonUser(string id);
        ApplicationUser Create(User user,string password);
		/// <summary>
		/// 创建一个新的用户
		/// </summary>
		/// <param name="user"></param>
		/// <param name="password"></param>
		/// <returns></returns>
        Task<ApplicationUser> CreateAsync(User user,string password);
		/// <summary>
		/// 修改用户信息
		/// </summary>
		/// <param name="user"></param>
		/// <param name="update"></param>
		/// <returns></returns>
		Task<User> ModefyAsync(User user,bool update);

		bool Edit(User newUser);

        Task<bool> EditAsync(User newUser);
        bool Remove(string id);
        Task<bool> RemoveAsync(string id);
    }
}
