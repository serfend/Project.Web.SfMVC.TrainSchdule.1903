using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
	public partial class UsersService : IUsersService
    {
        #region Fields

        private readonly ICurrentUserService _currentUserService;
        private readonly ApplicationDbContext _context;


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
			if(id.ToLower()=="root")return new User()
			{
				Id = "Root",
				Application = new UserApplicationInfo()
				{
					Permission = new Permissions()
					{
						Role= "Admin"
					}
				},
				BaseInfo = new UserBaseInfo()
				{
					RealName = "系统管理员"
				},
				CompanyInfo = new UserCompanyInfo()
				{
					Company = new Company()
					{
						Name = "Root"
					},
					Duties = new Duties()
					{
						Name = "Root"
					}
				},
				SocialInfo = new UserSocialInfo()
				{
					Address = new AdminDivision(),

				}
			};
            var user = _context.AppUsers.Find(id);

            return user;
        }

        public IQueryable<User> Find(Expression<Func<User, bool>> predict)
        {
			return _context.AppUsers.Where(predict).OrderBy(u=>u.BaseInfo.RealName);
        }

        public ApplicationUser ApplicaitonUser(string id)
        {
			return _context.Users.FirstOrDefault(u => u.UserName == id);
		}


        /// <summary>
        /// Creates user.
        /// </summary>
        public ApplicationUser Create(User user,string password) =>
	        CreateAsync(user,password).Result;

		/// <summary>
		/// Async creates user.
		/// </summary>
		public async Task<ApplicationUser> CreateAsync(User user,string password)
		{
			var identity = CreateUser(user, password);
			if (identity == null) return null;
			var appUser = CreateAppUser(user);
			
			await _context.Users.AddAsync(identity).ConfigureAwait(false);
			await _context.AppUsers.AddAsync(appUser).ConfigureAwait(false);
			await _context.SaveChangesAsync().ConfigureAwait(false);
			return identity;
        }

		private User CreateAppUser(User user)
		{
					
			user.Application.Create = DateTime.Now;
			user.Application.AuthKey = new Random().Next(1000, 9999).ToString().GetHashCode().ToString();
			user.Application.ApplicationSetting = new UserApplicationSetting()
			{
				LastSubmitApplyTime = DateTime.Now
			};
			user.Application.Permission = new Permissions()
			{
				Regions = "",
				Role = "User"
			};
			user.CompanyInfo.Company = _context.Companies.Find(user.CompanyInfo.Company.Code);
			user.CompanyInfo.Duties = _context.Duties.FirstOrDefault(d => d.Name == user.CompanyInfo.Duties.Name);
			user.SocialInfo.Address = _context.AdminDivisions.Find(user.SocialInfo?.Address?.Code);
			return user;
		}
		private ApplicationUser CreateUser(User user,string password)
		{
			if (_context.AppUsers.Find(user.Id) != null) return null;
			
			var identity = new ApplicationUser
			{
				UserName = user.Id,
				Email = user.Application.Email,
				PhoneNumberConfirmed = false,
				EmailConfirmed = false,
				NormalizedEmail = user.Application.Email.ToUpper(),
				NormalizedUserName = user.Id.ToUpper(),
				LockoutEnabled = true,
				TwoFactorEnabled = false,
				SecurityStamp = user.Id.GetHashCode().ToString()
			};

			var passwordHasher = new PasswordHasher<ApplicationUser>();
			identity.PasswordHash = passwordHasher.HashPassword(identity, password);


			return identity;
		}
        /// <summary>
        /// Edits user.
        /// </summary>
        public bool Edit(User newUser)
        {
			
			_context.AppUsers.Update(newUser);
			_context.SaveChanges();
			return true;
		}

        /// <summary>
        /// Async edits user.
        /// </summary>
        public async Task<bool> EditAsync(User newUser)
        {
	        _context.AppUsers.Update(newUser);
			await _context.SaveChangesAsync();
			return true;
		}



        public bool Remove(string id)
        {
			var user =  _context.AppUsers.Find(id);
			if (user == null) return false;
			_context.AppUsers.Remove(user);
			var appUser =  _context.Users.FirstOrDefault(u=>u.UserName==id);
			if (appUser == null) return false;
			_context.Users.Remove(appUser);
			_context.SaveChanges();
			return true;
		}
        public async Task<bool> RemoveAsync(string id)
        {
	        var user = await _context.AppUsers.FindAsync(id).ConfigureAwait(false);
	        if (user == null) return false;
			//TODO 级联删除相关
	        _context.AppUsers.Remove(user);
	        var appUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName==id).ConfigureAwait(false);
	        _context.Users.Remove(appUser);
			await _context.SaveChangesAsync().ConfigureAwait(false);
	        return true;
        }
		#endregion



    }
}