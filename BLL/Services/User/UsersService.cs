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
		public User GetById(string id)
		{
			if (id == null) return null;
			switch (id.ToLower())
			{
				case "root": return UserRoot();
				case "audit_skipper": return UserSkip();
			}
			var user = _context.AppUsers.Where(u => u.Id == id).FirstOrDefault();
			return user;
		}

		/// <summary>
		/// 系统管理员
		/// </summary>
		/// <returns></returns>
		private static User UserRoot()
		{
			return new User()
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
		}

		/// <summary>
		/// 审批流无合适人可审批时，使用此账号
		/// </summary>
		/// <returns></returns>
		private static User UserSkip()
		{
			return new User()
			{
				Id = "audit_skipper",
				Application = new UserApplicationInfo()
				{
					Permission = new Permissions()
					{
						Role = "User"
					}
				},
				BaseInfo = new UserBaseInfo()
				{
					RealName = "跳过审批"
				},
				CompanyInfo = new UserCompanyInfo()
				{
					Company = new Company()
					{
						Name = "跳过审批",
						Code = "root"
					},
					Duties = new Duties()
					{
						Name = "跳过审批",
						Code = 0
					}
				},
				SocialInfo = new UserSocialInfo()
				{
					Address = new AdminDivision(),
					Id = Guid.Empty
				}
			};
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
			var lastCreateTime = user?.Application?.Create;
			var appUser = CreateAppUser(user);
			appUser.Application.Create = lastCreateTime; // create time should not modify
			if (update)
			{
				_context.AppUsers.Update(appUser);
				await _context.SaveChangesAsync().ConfigureAwait(true);
			}
			return appUser;
		}

		public async Task RemoveNoRelateInfo()
		{
			// 所有在用的用户
			var users = _context.AppUsers;
			// 删除没有引用了的子表项（脏数据）
			var list_app = _context.AppUserApplicationInfos.Where(a => !users.Any(u => u.Application.Id == a.Id));
			_context.AppUserApplicationInfos.RemoveRange(list_app);
			var list_cmp = _context.AppUserCompanyInfos.Where(a => !users.Any(u => u.CompanyInfo.Id == a.Id));
			_context.AppUserCompanyInfos.RemoveRange(list_cmp);
			var appliesBaseInfo = _context.ApplyBaseInfos;
			var list_social = _context.AppUserSocialInfos.Where(a => !users.Any(u => u.SocialInfo.Id == a.Id)).Where(a => !appliesBaseInfo.Any(b => b.Social.Id == a.Id));
			_context.AppUserSocialInfos.RemoveRange(list_social);
			var list_base = _context.AppUserBaseInfos.Where(a => !users.Any(u => u.BaseInfo.Id == a.Id));
			_context.AppUserBaseInfos.RemoveRange(list_base);
			var list_diy = _context.AppUserDiyInfos.Where(a => !users.Any(u => u.DiyInfo.Id == a.Id));
			_context.AppUserDiyInfos.RemoveRange(list_diy);
			await _context.SaveChangesAsync().ConfigureAwait(false);
		}

		private User CreateAppUser(User user)
		{
			user.Application.Create = DateTime.Now;
			user.Application.AuthKey = new Random().Next(1000, 99999).ToString().GetHashCode().ToString();
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

		public bool Remove(string id) => RemoveAsync(id).Result;

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
			RemoveUserInfo(user);
			var appUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.Id).ConfigureAwait(true);
			_context.Users.Remove(appUser);
		}

		private void RemoveUserInfo(User user)
		{
			if (user.BaseInfo != null)
			{
				_context.AppUserBaseInfos.Remove(user.BaseInfo);
			}
			if (user.Application != null)
			{
				if (user.Application.Permission != null) _context.Permissions.Remove(user.Application.Permission);
				if (user.Application.ApplicationSetting != null) _context.AppUserApplicationSettings.Remove(user.Application.ApplicationSetting);
			}
			if (user.CompanyInfo != null) _context.AppUserCompanyInfos.Remove(user.CompanyInfo);

			if (user.SocialInfo != null)
			{
				_context.AppUserSocialInfos.Remove(user.SocialInfo);
				if (user.SocialInfo.Settle != null)
				{
					_context.AppUserSocialInfoSettles.Remove(user.SocialInfo.Settle);
					if (user.SocialInfo.Settle.Self != null) _context.AppUserSocialInfoSettleMoments.Remove(user.SocialInfo.Settle.Self);
					if (user.SocialInfo.Settle.Parent != null) _context.AppUserSocialInfoSettleMoments.Remove(user.SocialInfo.Settle.Parent);
					if (user.SocialInfo.Settle.Lover != null) _context.AppUserSocialInfoSettleMoments.Remove(user.SocialInfo.Settle.Lover);
					if (user.SocialInfo.Settle.LoversParent != null) _context.AppUserSocialInfoSettleMoments.Remove(user.SocialInfo.Settle.LoversParent);
				}
			}
			if (user.DiyInfo != null) _context.AppUserDiyInfos.Remove(user.DiyInfo);
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
				throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.StaticMessage.FileSizeInvalid, $"最大支持200KB,当前:{avatar.Img.Length}", true));
			}
			return avatar;
		}

		public IEnumerable<Avatar> QueryAvatar(string targetUser, DateTime start)
		{
			if (targetUser == null) return null;
			var list = _context.AppUserDiyAvatars.Where(a => a.CreateTime >= start).Where(a => a.User.Id == targetUser);
			return list;
		}
	}

	#endregion Logic
}