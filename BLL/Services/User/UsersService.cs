using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Services.ApplyServices;
using DAL.Data;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
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

		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly ApplicationDbContext _context;
		private readonly IVacationCheckServices _vacationCheckServices;

		#endregion Fields

		#region .ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="UsersService"/>.
		/// </summary>
		public UsersService(IVacationCheckServices vacationCheckServices, ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
		{
			_context = context;
			_hostingEnvironment = hostingEnvironment;
			_vacationCheckServices = vacationCheckServices;
		}

		#endregion .ctors

		#region Logic

		/// <summary>
		/// Loads all users with paggination, returns collection of user DTOs.
		/// </summary>
		public IEnumerable<User> GetAll(int page, int pageSize)
		{
			var users = _context.AppUsers.Skip(page * pageSize).Take(pageSize);
			return users.ToList();
		}

		/// <summary>
		/// Loads user by username, returns user DTO.
		/// </summary>
		public User Get(string id)
		{
			if (id == null) return null;
			if (id.ToLower() == "root") return new User()
			{
				Id = "root",
				Application = new UserApplicationInfo()
				{
					Permission = new Permissions()
					{
						Role = "admin"
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
						Name = "系统管理",
						Code = "root"
					},
					Duties = new Duties()
					{
						Name = "系统管理",
						Code = 0
					}
				},
				SocialInfo = new UserSocialInfo()
				{
					Address = new AdminDivision(),
					Id = Guid.Empty
				}
			};
			var user = _context.AppUsers.Where(u => u.Id == id).FirstOrDefault();
			return user;
		}

		public IQueryable<User> Find(Expression<Func<User, bool>> predict)
		{
			return _context.AppUsers.Where(predict).OrderBy(u => u.BaseInfo.RealName);
		}

		public ApplicationUser ApplicaitonUser(string id)
		{
			return _context.Users.FirstOrDefault(u => u.UserName == id);
		}

		/// <summary>
		/// Creates user.
		/// </summary>
		public ApplicationUser Create(User user, string password) =>
			CreateAsync(user, password).Result;

		/// <summary>
		/// Async creates user.
		/// </summary>
		public async Task<ApplicationUser> CreateAsync(User user, string password)
		{
			if (user == null) return null;
			var identity = CreateUser(user, password);
			if (identity == null) return null;
			var appUser = CreateAppUser(user);

			await _context.Users.AddAsync(identity).ConfigureAwait(true);
			await _context.AppUsers.AddAsync(appUser).ConfigureAwait(true);
			await _context.SaveChangesAsync().ConfigureAwait(true);

			return identity;
		}

		public async Task<User> ModefyAsync(User user, bool update)
		{
			if (user == null) return null;
			var appUser = CreateAppUser(user);
			if (update)
			{
				_context.AppUsers.Update(appUser);
				await _context.SaveChangesAsync().ConfigureAwait(true);
			}
			return appUser;
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
			var title = user.CompanyInfo.Title;
			user.CompanyInfo.Title = _context.UserCompanyTitles.FirstOrDefault(d => d.Name == title.Name);
			var social = user.SocialInfo;
			social.Address = _context.AdminDivisions.Find(user.SocialInfo?.Address?.Code);
			if (social.Settle?.Lover?.Address != null) social.Settle.Lover.Address = _context.AdminDivisions.Find(social.Settle.Lover.Address.Code);
			if (social.Settle?.Parent?.Address != null) social.Settle.Parent.Address = _context.AdminDivisions.Find(social.Settle.Parent.Address.Code);
			if (social.Settle?.LoversParent?.Address != null) social.Settle.LoversParent.Address = _context.AdminDivisions.Find(social.Settle.LoversParent.Address.Code);
			if (social.Settle?.Self?.Address != null) social.Settle.Self.Address = _context.AdminDivisions.Find(social.Settle.Self.Address.Code);
			if (social.Settle != null) social.Settle.PrevYealyLengthHistory = new List<AppUsersSettleModefyRecord>();
			return user;
		}

		private ApplicationUser CreateUser(User user, string password)
		{
			if (_context.Users.Where(u => u.UserName == user.Id).FirstOrDefault() != null) return null;

			var identity = new ApplicationUser
			{
				UserName = user.Id,
				Email = user.Application?.Email,
				PhoneNumberConfirmed = false,
				EmailConfirmed = false,
				NormalizedEmail = user.Application?.Email?.ToUpper(),
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
			await _context.SaveChangesAsync().ConfigureAwait(true);
			return true;
		}

		public bool Remove(string id)
		{
			var user = _context.AppUsers.Find(id);
			if (user == null) return false;
			_context.AppUsers.Remove(user);
			var appUser = _context.Users.FirstOrDefault(u => u.UserName == id);
			if (appUser == null) return false;
			_context.Users.Remove(appUser);
			_context.SaveChanges();
			return true;
		}

		public async Task<bool> RemoveAsync(string id)
		{
			var user = await _context.AppUsers.FindAsync(id).ConfigureAwait(true);
			if (user == null) return false;
			await RemoveUser(user).ConfigureAwait(true);
			await _context.SaveChangesAsync().ConfigureAwait(true);
			return true;
		}

		/// <summary>
		/// 删除用户
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		private async Task RemoveUser(User user)
		{
			_context.AppUsers.Remove(user);
			if (user.BaseInfo != null)
			{
				_context.AppUserBaseInfos.Remove(user.BaseInfo);
			}
			if (user.Application != null)
			{
				if (user.Application.Permission != null) _context.Permissions.Remove(user.Application.Permission);
				if (user.Application.ApplicationSetting != null) _context.AppUserApplicationInfos.Remove(user.Application);
			}
			if (user.Application?.ApplicationSetting != null) _context.AppUserApplicationSettings.Remove(user.Application.ApplicationSetting);
			if (user.CompanyInfo != null) _context.AppUserCompanyInfos.Remove(user.CompanyInfo);
			if (user.SocialInfo != null)
			{
				_context.AppUserSocialInfos.Remove(user.SocialInfo);
				if (user.SocialInfo.Settle != null)
				{
					_context.AUserSocialInfoSettles.Remove(user.SocialInfo.Settle);
					if (user.SocialInfo.Settle.Self != null) _context.AppUserSocialInfoSettleMoments.Remove(user.SocialInfo.Settle.Self);
					if (user.SocialInfo.Settle.Parent != null) _context.AppUserSocialInfoSettleMoments.Remove(user.SocialInfo.Settle.Parent);
					if (user.SocialInfo.Settle.Lover != null) _context.AppUserSocialInfoSettleMoments.Remove(user.SocialInfo.Settle.Lover);
					if (user.SocialInfo.Settle.LoversParent != null) _context.AppUserSocialInfoSettleMoments.Remove(user.SocialInfo.Settle.LoversParent);
				}
			}
			if (user.DiyInfo != null) _context.AppUserDiyInfos.Remove(user.DiyInfo);
			var appUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.Id).ConfigureAwait(true);
			_context.Users.Remove(appUser);
		}

		public string ConvertFromUserCiper(string username, string password)
		{
			return password.FromCipperToString(username);
		}

		public async Task<Avatar> UpdateAvatar(User targetUser, string newAvatar)
		{
			if (targetUser == null) return null;
			var now = DateTime.Now;
			var avatar = new Avatar()
			{
				FilePath = newAvatar == null ? null : $"{Guid.NewGuid().ToString()}.png",
				CreateTime = now,
				Img = newAvatar.FromBase64ToBytes(),
				User = targetUser
			};
			if (avatar?.Img?.Length <= 1024 * 200)
			{
				if (targetUser.DiyInfo != null)
				{
					_context.AppUserDiyAvatars.Add(avatar);
					targetUser.DiyInfo.Avatar = avatar;
					_context.AppUserDiyInfos.Update(targetUser.DiyInfo);
					await _context.SaveChangesAsync().ConfigureAwait(true);
				}
			}
			else if (avatar?.Img?.Length > 1024 * 200)
			{
				throw new FileTooLargeException(1024 * 200, avatar.Img.Length);
			}
			return avatar;
		}

		public IEnumerable<Avatar> QueryAvatar(string targetUser, DateTime start)
		{
			if (targetUser == null) return null;
			var list = _context.AppUserDiyAvatars.Where(a => a.CreateTime >= start).Where(a => a.User.Id == targetUser);
			return list;
		}

		#endregion Logic

		/// <summary>
		/// 文件尺寸
		/// </summary>
		[Serializable]
		public class FileTooLargeException : Exception
		{
			public FileTooLargeException(int requireSize, int currentSize) : this($"要求尺寸在{Math.Round((double)requireSize / 1000000, 2)}MB以内,实际{Math.Round((double)currentSize / 1000000, 2)}MB")
			{
			}

			public FileTooLargeException(string message) : base(message)
			{
			}

			public FileTooLargeException(string message, Exception inner) : base(message, inner)
			{
			}

			protected FileTooLargeException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
		}
	}
}