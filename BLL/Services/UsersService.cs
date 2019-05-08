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
			if(id=="Root")return new User()
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
        public ApplicationUser Create(UserCreateVdto user) =>
	        CreateAsync(user).Result;

		/// <summary>
		/// Async creates user.
		/// </summary>
		public async Task<ApplicationUser> CreateAsync(UserCreateVdto user)
		{
			var identity = CreateUser(user);
			if (identity == null) return null;
			var appUser = CreateAppUser(user);
			
			await _context.Users.AddAsync(identity);
			await _context.AppUsers.AddAsync(appUser);
			await _context.SaveChangesAsync();
			return identity;
        }

		private User CreateAppUser(UserCreateVdto user)
		{
			var u = new User()
			{
				Id = user.Id,
				Application = new UserApplicationInfo()
				{
					Email = user.Email,
					Permission = new Permissions()
					{
						Regions = "",
						Role = "User"
					},
					InvitedBy = user.InvitedBy,
					Create = DateTime.Now,
					AuthKey = user.Id.GetHashCode().ToString(),
					ApplicationSetting = new UserApplicationSetting()
					{
						LastSubmitApplyTime = DateTime.Now
					}
				},
				BaseInfo = new UserBaseInfo()
				{
					Gender = user.Gender,
					RealName = user.RealName
				},
				CompanyInfo = new UserCompanyInfo()
				{
					Company = _context.Companies.Find(user.Company),
					Duties = _context.Duties.SingleOrDefault(d=>d.Name==user.Duties)
				},
				SocialInfo = new UserSocialInfo()
				{
					Address = _context.AdminDivisions.Find(user.HomeAddress),
					AddressDetail = user.HomeDetailAddress,
					Phone = user.Phone
				}
			};
			
			return u;
		}
		private ApplicationUser CreateUser(UserCreateVdto user)
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
			var appUser =  _context.Users.SingleOrDefault(u=>u.UserName==id);
			if (appUser == null) return false;
			_context.Users.Remove(appUser);
			_context.SaveChanges();
			return true;
		}
        public async Task<bool> RemoveAsync(string id)
        {
	        var user = await _context.AppUsers.FindAsync(id);
	        if (user == null) return false;
			//TODO 级联删除相关
	        _context.AppUsers.Remove(user);
	        var appUser = await _context.Users.SingleOrDefaultAsync(u => u.UserName==id);
	        _context.Users.Remove(appUser);
			await _context.SaveChangesAsync();
	        return true;
        }
		#endregion



    }
}