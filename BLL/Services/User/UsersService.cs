using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Services.ApplyServices;
using DAL.Data;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
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

		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly ApplicationDbContext _context;
		private readonly IVacationCheckServices _vacationCheckServices;

		#endregion Fields

		#region .ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="UsersService"/>.
		/// </summary>
		public UsersService(IVacationCheckServices vacationCheckServices, ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
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
			var users = _context.AppUsersDb.Skip(page * pageSize).Take(pageSize);
			return users.ToList();
		}
		public IQueryable<User> GetUserByRealname(string realName)
        {
			if (realName == null) throw new ActionStatusMessageException(ActionStatusMessage.UserMessage.NoId);
			realName = realName.Replace(" ", "");
			var realNameWithSpace = realName.Length==2? $"{realName[0]}  {realName[1]}":null;
			var isAdmin = realName.ToLower() == "admin";
			var users = isAdmin ? new List<User>() { GetById("root") }.AsQueryable() : (realName.Length==2? _context.AppUsers.Where(u => u.BaseInfo.RealName == realName || u.BaseInfo.RealName == realNameWithSpace) : _context.AppUsers.Where(u => u.BaseInfo.RealName == realName));
			if (!users.Any()) users = _context.AppUsers.Where(u => u.BaseInfo.RealName.Contains(realName));
			if (!isAdmin) users = users.OrderByCompanyAndTitle();
			return users;
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
			var user = _context.AppUsersDb.Where(u => u.Id == id).FirstOrDefault();
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
			return _context.AppUsersDb.Where(predict).OrderBy(u => u.BaseInfo.RealName);
		}

		public ApplicationUser ApplicaitonUser(string id)
		{
			return _context.Users.FirstOrDefault(u => u.UserName == id);
		}

		/// <summary>
		/// Creates user.
		/// </summary>
		public ApplicationUser Create(User user, string password, Func<User, bool> checkUserValid) =>
			CreateAsync(user, password, checkUserValid).Result;

		/// <summary>
		/// Async creates user.
		/// </summary>
		public async Task<ApplicationUser> CreateAsync(User user, string password, Func<User, bool> checkUserValid)
		{
			if (user == null) throw new ActionStatusMessageException(ActionStatusMessage.Account.Register.UserInvalid);
			var identity = CreateUser(user, password);
			if (identity == null) throw new ActionStatusMessageException(ActionStatusMessage.Account.Register.IdentityFail);
			var appUser = CreateAppUser(user);
			if (!checkUserValid?.Invoke(appUser) ?? true) return null;
			await _context.Users.AddAsync(identity).ConfigureAwait(true);
			await _context.AppUsers.AddAsync(appUser).ConfigureAwait(true);
			await _context.SaveChangesAsync().ConfigureAwait(true);

			return identity;
		}

		public async Task<User> ModifyAsync(User newUser, bool update)
		{
			if (newUser == null) return null;
			var lastCreateTime = newUser?.Application?.Create;
			var appUser = CreateAppUser(newUser);
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
			user.CompanyInfo.Company = _context.CompaniesDb.FirstOrDefault(c => c.Code == user.CompanyInfo.CompanyCode);
			user.CompanyInfo.Duties = _context.Duties.FirstOrDefault(d => d.Name == user.CompanyInfo.Duties.Name);
			var title = user.CompanyInfo.Title;
			user.CompanyInfo.Title = _context.UserCompanyTitles.FirstOrDefault(d => d.Name == title.Name);
			var social = user.SocialInfo;
			social.Address = _context.AdminDivisions.Find(user.SocialInfo?.Address?.Code);
			if (social.Settle != null)
			{
				if (social.Settle.Lover?.Address != null) social.Settle.Lover.Address = _context.AdminDivisions.Find(social.Settle.Lover.Address.Code);
				if (social.Settle.Parent?.Address != null) social.Settle.Parent.Address = _context.AdminDivisions.Find(social.Settle.Parent.Address.Code);
				if (social.Settle.LoversParent?.Address != null) social.Settle.LoversParent.Address = _context.AdminDivisions.Find(social.Settle.LoversParent.Address.Code);
				if (social.Settle.Self?.Address != null) social.Settle.Self.Address = _context.AdminDivisions.Find(social.Settle.Self.Address.Code);
				// if prev yealy history not set , then build new one
				if (social.Settle.PrevYealyLengthHistory == null) social.Settle.PrevYealyLengthHistory = new List<AppUsersSettleModifyRecord>();
			}
			return user;
		}

		private ApplicationUser CreateUser(User user, string password)
		{
			if (_context.Users.Where(u => u.UserName == user.Id).FirstOrDefault() != null)
            {
				var check_removed_user = _context.AppUsersDb.Where(u => u.Id == user.Id) == null?"原用户已被移除，如需继续使用，请联系管理员恢复。":null;
				throw new ActionStatusMessageException(ActionStatusMessage.Account.Register.AccountExist, check_removed_user);
			}

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

		public bool Remove(string id,string reason, bool RemoveEntity = false) => RemoveAsync(id, reason, RemoveEntity).Result;

		public bool RestoreUser(string id)
		{
			var user = _context.AppUsers.FirstOrDefault(u => u.Id == id);
			if (user == null) return false;
			if (((int)user.AccountStatus & (int)AccountStatus.Abolish) > 0)
			{
				user.AccountStatus -= (int)AccountStatus.Abolish;
				SetUserAppliesStatus(id, false);
				_context.AppUsers.Update(user);
			}
			return true;
		}

		private void SetUserAppliesStatus(string id, bool abolish)
		{
			var user_applies = _context.Applies.Where(a => a.BaseInfo.FromId == id);
			foreach (var a in user_applies)
			{
				if(abolish)
					a.MainStatus |= MainStatus.Invalid;
                else if (((int)a.MainStatus & (int)MainStatus.Invalid) > 0)
					a.MainStatus -= MainStatus.Invalid;
			}
			_context.Applies.UpdateRange(user_applies);
		}

		public async Task<bool> RemoveAsync(string id,string reason, bool RemoveEntity = false)
		{
			var user = await _context.AppUsersDb.FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(true);
			if (user == null) return false;
			RemoveUser(user, reason, RemoveEntity);
			await _context.SaveChangesAsync().ConfigureAwait(true);
			return true;
		}

		/// <summary>
		/// 删除用户
		/// 20201019@serfend:改为仅修改用户删除属性为已删除
		/// 20201021@serfend:保留两种删除方式
		/// </summary>
		/// <param name="user"></param>
		/// <param name="reason"></param>
		/// <param name="RemoveEntity">是否完全删除</param>
		/// <returns></returns>
		private void RemoveUser(User user,string reason, bool RemoveEntity = false)
		{
			if (!RemoveEntity)
			{
				user.AccountStatus += (int)AccountStatus.Abolish;
				user.Application.UserRemoveReason = reason;
				_context.AppUsers.Update(user);
				SetUserAppliesStatus(user.Id, true);
			}
			else
			{
				_context.AppUsers.Remove(user);
				RemoveUserInfo(user);
				var appUser = _context.Users.FirstOrDefault(u => u.UserName == user.Id);
				_context.Users.Remove(appUser);
			}
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
			if (user.DiyInfo != null)
			{
				_context.AppUserDiyInfos.Remove(user.DiyInfo);
				if (user.DiyInfo.Avatar != null) _context.AppUserDiyAvatars.Remove(user.DiyInfo.Avatar);
			}
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
			var list = _context.AppUserDiyAvatars.Where(a => a.CreateTime >= start).Where(a => a.UserId == targetUser);
			return list;
		}
	}

	#endregion Logic
}