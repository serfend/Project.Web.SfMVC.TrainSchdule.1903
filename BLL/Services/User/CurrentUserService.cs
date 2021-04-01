using Abp.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace BLL.Services
{
	/// <summary>
	/// Contains properties that returns current user.
	/// Realization of <see cref="ICurrentUserService"/>.
	/// </summary>
	public class CurrentUserService : ICurrentUserService
	{
		#region Fields

		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ApplicationDbContext _context;

		#endregion Fields

		#region Properties

		/// <returns>
		/// Returns current user entity.
		/// </returns>
		public User CurrentUser =>
			_context.AppUsersDb
				.FirstOrDefault(u => u.Id == _httpContextAccessor.HttpContext.User.Identity.Name);

		public IHttpContextAccessor HttpContextAccessor => _httpContextAccessor;

		#endregion Properties

		#region .ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="CurrentUserService"/>.
		/// </summary>
		public CurrentUserService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
		{
			_httpContextAccessor = httpContextAccessor;
			_context = context;
		}


        #endregion .ctors
    }
}