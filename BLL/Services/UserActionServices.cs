using BLL.Interfaces;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
	public class UserActionServices : IUserActionServices
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ApplicationDbContext _context;

		public UserActionServices(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
		{
			_httpContextAccessor = httpContextAccessor;
			_context = context;
		}

		public UserAction Log(UserOperation operation,string username,string description,bool success=false)
		{
			var context = _httpContextAccessor.HttpContext;
			var ua = new UserAction()
			{
				Date = DateTime.Now,
				Device = context.Request.Query["device"],
				UA = context.Request.Headers["User-Agent"],
				Operation = operation,
				UserName = username,
				Ip = context.Connection.RemoteIpAddress.ToString(),
				Success= success,
				Description= description
			};
			_context.UserActions.Add(ua);
			_context.SaveChanges();
			return ua;
		}

		public bool Permission(Permissions permissions, PermissionDescription key, Operation operation, string permissionUserName,string targetUserCompanyCode)
		{
			var a=Log(UserOperation.Permission, permissionUserName, $"授权到{targetUserCompanyCode}执行{key?.Name} {key?.Description}");
			if (permissions.Check(key, operation, targetUserCompanyCode))
			{
				Success(a);
				return true;
			}
			return false;
		}

		public UserAction Success(UserAction action)
		{
			action.Success = true;
			_context.UserActions.Update(action);
			_context.SaveChanges();
			return action;
		}
	}
}
