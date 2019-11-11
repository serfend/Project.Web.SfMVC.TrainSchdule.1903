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
        /// <summary>
        /// 加载所有用户的信息
        /// </summary>
        IEnumerable<User> GetAll(int page, int pageSize);

        User Get(string Id);

        IQueryable<User> Find(Expression<Func<User, bool>>predict);

        ApplicationUser ApplicaitonUser(string id);
        ApplicationUser Create(User user,string password);

        Task<ApplicationUser> CreateAsync(User user,string password);

        bool Edit(User newUser);

        Task<bool> EditAsync(User newUser);
        bool Remove(string id);
        Task<bool> RemoveAsync(string id);

		

    }
}
