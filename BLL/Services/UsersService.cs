using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.UserInfo;
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

		private const string privateKey = "MIICXgIBAAKBgQCz3lBSqu05sK6r3SDCr2z0lT19j4LBbWbapEvv37paxbwmkvA5E/nr/VD9Hw2jueBt9NyEdnzWEgN+WmRF1GUYBQFL6YWneFkovgpLA8tgXHEojePAMfgMb+hoYHoV90MUQwANDbt0gg4nnlRxZB+WtZc5CUQT5x7ckCs5+iQNTwIDAQABAoGBAKSOgcH/6uTaxhMqTWyP/giN+SHEiAXaxzzFD0w3zVB6kzZfPDOcGQxURyIspNfjmHZAjPcLSA65kESrAg340Trs00k9i1JfzYp4hc/r85yBVTp5ljWp8kPWRpfJBK3yzBok4qvGbIpJHlLrENFnVUd0dkPXKaOXZs3+mZ1GWTIRAkEA5gFj/QkpGWa/PRLSJ55ptdiIVjxXDhdNVJsozs4UcbYr/CIEUiQA6OqYNOWr8shAdQM1g65PvDYWGFJQq42qvQJBAMgyVr5P1Vj2EwahnbDtD9Zzngchcv5sv9sVlI3NNhD4tkzxntc01ikOzzy9M+x3cP1tHavv8lxgNWnWAi6hnvsCQBprfnjKXJY2XzE8wDcc0ze4L7D4LWfI9XEKgZ1/volxS4wivCxTRmd6yxEIcL/qkLzgKX1+wFn2PIN+sRWDqGECQQCazbofvXHXMajypsReTGHDzXF0SBw4uvT8P0q4/+b/5qJpCyltdjDoXMhJSnC9OHsJrHeWPZvmbIrBBTh4wIdDAkEAirO5n1K88opa8chywQxfdnKqt0BJq/x+Xp2W9V4p61PucMKSDQQ3Ytf47JUfi/17WeQeTc5L6RBDxVF2Hqjk4Q==";
		private const string publicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCz3lBSqu05sK6r3SDCr2z0lT19j4LBbWbapEvv37paxbwmkvA5E/nr/VD9Hw2jueBt9NyEdnzWEgN+WmRF1GUYBQFL6YWneFkovgpLA8tgXHEojePAMfgMb+hoYHoV90MUQwANDbt0gg4nnlRxZB+WtZc5CUQT5x7ckCs5+iQNTwIDAQAB";

		public string ConvertFromUserCiper(string username, string rawPsw)
		{
			using (var md5 = MD5.Create())
			{
				var result = md5.ComputeHash(Encoding.UTF8.GetBytes(username));
				var rawMd5 = BitConverter.ToString(result);
				string md5Str = rawMd5.Replace("-", "");
				var rsa = new RsaHelper(RSAType.RSA2, Encoding.UTF8, privateKey, publicKey);
				var decryptStr = rsa.Decrypt(rawPsw);
				if (decryptStr==null||decryptStr.Length <= md5Str.Length || 
					DateTime.Now.ToString("yyyyMMdd") != decryptStr.Substring(0, 8) || 
					decryptStr.Substring(decryptStr.Length - md5Str.Length, md5Str.Length) != md5Str.ToLower()) return null;
				return decryptStr.Substring(8, decryptStr.Length - 8 - md5Str.Length);

			}
		}
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
				Id = "Root",
				Application = new UserApplicationInfo()
				{
					Permission = new Permissions()
					{
						Role = "Admin"
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
			var social = user.SocialInfo;
			social.Address = _context.AdminDivisions.Find(user.SocialInfo?.Address?.Code);
			if (social.Settle?.Lover?.Address != null) social.Settle.Lover.Address = _context.AdminDivisions.Find(social.Settle.Lover.Address.Code);
			if (social.Settle?.Parent?.Address != null) social.Settle.Parent.Address = _context.AdminDivisions.Find(social.Settle.Parent.Address.Code);
			if (social.Settle?.LoversParent?.Address != null) social.Settle.LoversParent.Address = _context.AdminDivisions.Find(social.Settle.LoversParent.Address.Code);
			if (social.Settle?.Self?.Address != null) social.Settle.Self.Address = _context.AdminDivisions.Find(social.Settle.Self.Address.Code);
			return user;
		}
		private ApplicationUser CreateUser(User user, string password)
		{
			if (_context.Users.Where(u => u.UserName == user.Id).FirstOrDefault() != null) return null;

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
			var user = await _context.AppUsers.FindAsync(id).ConfigureAwait(false);
			if (user == null) return false;
			//TODO 级联删除相关(休假申请）
			_context.AppUsers.Remove(user);
			if(user.BaseInfo!=null)_context.AppUserBaseInfos.Remove(user.BaseInfo);
			if (user.Application!=null)_context.AppUserApplicationInfos.Remove(user.Application);
			if(user.Application?.ApplicationSetting!=null)_context.AppUserApplicationSettings.Remove(user.Application.ApplicationSetting);
			if (user.CompanyInfo != null) _context.AppUserCompanyInfos.Remove(user.CompanyInfo);
			if (user.CompanyInfo?.Company != null) _context.Companies.Remove(user.CompanyInfo.Company);
			if (user.CompanyInfo?.Duties != null) _context.Duties.Remove(user.CompanyInfo.Duties);

			var appUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == id).ConfigureAwait(false);
			_context.Users.Remove(appUser);
			await _context.SaveChangesAsync().ConfigureAwait(false);
			return true;
		}
		#endregion



	}
}