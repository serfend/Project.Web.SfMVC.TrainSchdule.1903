using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.User;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Permission;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using User = DAL.Entities.UserInfo.User;

namespace BLL.Services
{
	/// <summary>
	/// Contains methods with users processing logic.
	/// Realization of <see cref="IUsersService"/>.
	/// </summary>
	public class UsersService : IUsersService
    {
        #region Fields

        private readonly ICurrentUserService _currentUserService;
        private readonly ApplicationDbContext _context;

        private bool _isDisposed;

        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersService"/>.
        /// </summary>
        public UsersService(ICurrentUserService currentUserService, ApplicationDbContext context)
        {
	        _currentUserService = currentUserService;
	        _context = context;
        }

        #endregion

        #region Logic

        /// <summary>
        /// Loads all users with paggination, returns collection of user DTOs.
        /// </summary>
        public IEnumerable<User> GetAll(int page, int pageSize)
        {
            var users = _context.AppUsers.Skip(page*pageSize).Take(pageSize);
            return users.ToList();
        }

        /// <summary>
        /// Loads user by username, returns user DTO.
        /// </summary>
        public User Get(string id)
        {
            var user = _context.AppUsers.Find(id);

            return user;
        }

        public IQueryable<User> Find(Expression<Func<User, bool>> predict)
        {
			return _context.AppUsers.Where(predict).OrderBy(u=>u.BaseInfo.RealName);
        }


        /// <summary>
        /// Creates user.
        /// </summary>
        public ApplicationUser Create(UserCreateDTO user) =>
	        CreateAsync(user).Result;

		/// <summary>
		/// Async creates user.
		/// </summary>
		public async Task<ApplicationUser> CreateAsync(UserCreateDTO user)
		{
			var identity = CreateUser(user);
			var appUser = CreateAppUser(user);
			await _context.Users.AddAsync(identity);
			await _context.AppUsers.AddAsync(appUser);
            await _context.SaveChangesAsync();

            return identity;
        }

		private User CreateAppUser(UserCreateDTO user)
		{
			var u = new User()
			{
				Id = user.Id,
				Application = new UserApplicationInfo()
				{
					Email = user.Email,
					Permission = new Permissions(),
					InvitedBy = user.InvitedBy
				},
				BaseInfo = new UserBaseInfo()
				{
					AuthKey = user.Id.GetHashCode().ToString(),
					Gender = user.Gender,
					RealName = user.RealName
				},
				CompanyInfo = new UserCompanyInfo()
				{
					
				}
			};
			return u;
		}
		private ApplicationUser CreateUser(UserCreateDTO user)
		{
			if (_context.AppUsers.Find(user.Id) != null) return null;

			var identity = new ApplicationUser
			{
				UserName = user.Id,
				Email = user.Email,
				PhoneNumberConfirmed = false,
				EmailConfirmed = false,
				NormalizedEmail = user.Email.ToUpper(),
				NormalizedUserName = user.Id.ToUpper(),
				LockoutEnabled = true,
				TwoFactorEnabled = false,
				SecurityStamp = user.Id.GetHashCode().ToString()
			};

			var passwordHasher = new PasswordHasher<ApplicationUser>();
			identity.PasswordHash = passwordHasher.HashPassword(identity, user.Password);


			return identity;
		}
        /// <summary>
        /// Edits user.
        /// </summary>
        public bool Edit(string id, Action<User> editCallBack)
        {
	        var user = _context.AppUsers.Find(id);
            if (user == null) return false;
            editCallBack.Invoke(user);
			_context.AppUsers.Update(user);
			_context.SaveChanges();
            return true;
		}

        /// <summary>
        /// Async edits user.
        /// </summary>
        public async Task<bool> EditAsync(string id, Action<User> editCallBack)
        {
			var user = _context.AppUsers.Find(id);
			if (user == null) return false;
			await Task.Run(() => editCallBack.Invoke(user));
			_context.AppUsers.Update(user);
			await _context.SaveChangesAsync();
			return true;
		}

        #endregion


        #region Disposing

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _currentUserService.Dispose();
                }

                _isDisposed = true;
            }
        }

        ~UsersService()
        {
            Dispose(false);
        }

        #endregion
    }
}