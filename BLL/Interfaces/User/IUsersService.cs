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
	public interface IUsersService : IUserServiceDetail
	{

		/// <summary>
		/// 返回当前查询用户，或当前用户
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		User CurrentQueryUser(string id);
		/// <summary>
		/// 通过姓名获取账号（排除空格）
		/// </summary>
		/// <param name="realName"></param>
		/// <param name="fuzz"></param>
		/// <returns></returns>
		IQueryable<User> GetUserByRealname(string realName,bool fuzz);
		/// <summary>
		/// 加载所有用户的信息
		/// </summary>
		IEnumerable<User> GetAll(int page, int pageSize);

		User GetById(string Id);
		IQueryable<User> Find(Expression<Func<User, bool>> predict);

		ApplicationUser ApplicaitonUser(string id);

		ApplicationUser Create(User user, string password, Func<User, bool> checkUserValid);

		/// <summary>
		/// 创建一个新的用户
		/// </summary>
		/// <param name="user"></param>
		/// <param name="password"></param>
		/// <param name="checkUserValid">检查用户是否有效的回调</param>
		/// <returns></returns>
		Task<ApplicationUser> CreateAsync(User user, string password, Func<User, bool> checkUserValid);

		/// <summary>
		/// 修改用户信息
		/// </summary>
		/// <param name="newUser"></param>
		/// <param name="update"></param>
		/// <returns></returns>
		Task<User> ModifyAsync(User newUser, bool update);

		bool Edit(User newUser);

		Task<bool> EditAsync(User newUser);

		/// <summary>
		/// 删除用户
		/// </summary>
		/// <param name="id"></param>
		/// <param name="reason"></param>
		/// <param name="RemoveEneity">是否完全删除</param>
		/// <returns></returns>
		bool Remove(string id,string reason, bool RemoveEneity = false);

		/// <summary>
		/// 恢复已删除的用户
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		bool RestoreUser(string id);

		/// <summary>
		/// 删除用户
		/// </summary>
		/// <param name="id"></param>
		/// <param name="reason"></param>
		/// <param name="RemoveEneity">是否完全删除</param>
		/// <returns></returns>
		Task<bool> RemoveAsync(string id,string reason, bool RemoveEneity = false);

		/// <summary>
		/// 删除已经没有任何引用了的子表项
		/// </summary>
		/// <returns></returns>
		Task RemoveNoRelateInfo();
	}
}