using BLL.Interfaces;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public UserAction Log(UserOperation operation, string username, string description, bool success = false, ActionRank rank = ActionRank.Debug)
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
				Success = success,
				Description = description,
				Rank = rank
			};
			_context.UserActions.Add(ua);
			_context.SaveChanges();
			return ua;
		}

		public bool Permission(Permissions permissions, PermissionDescription key, Operation operation, string permissionUserName, string targetUserCompanyCode)
		{
			var a = Log(UserOperation.Permission, permissionUserName, $"授权到{targetUserCompanyCode}执行{key?.Name} {key?.Description}");
			if (permissions.Check(key, operation, targetUserCompanyCode))
			{
				Status(a, true, "授权成功");
				return true;
			}
			return false;
		}

		public async Task<IEnumerable<UserAction>> Query(QueryUserActionViewModel model)
		{
			if (model == null) return null;
			var r = _context.UserActions.AsQueryable();
			if (model.UserName?.Value != null) r = r.Where(h => h.UserName == (model.UserName.Value));
			if (model.Date?.Start != null && model.Date?.End != null)
			{
				r = r.Where(h => h.Date >= model.Date.Start).Where(h => h.Date <= model.Date.End);
			}
			if (model.Rank?.Arrays != null) r = r.Where(h => model.Rank.Arrays.Contains((int)h.Rank));
			if (model.Page == null) model.Page = new QueryByPage()
			{
				PageIndex = 0,
				PageSize = 20
			};
			return r.OrderByDescending(h => h.Date).Skip(model.Page.PageSize * model.Page.PageIndex).Take(model.Page.PageSize);
		}

		public UserAction Status(UserAction action, bool success, string description = null)
		{
			if (action == null) return null;
			action.Success = success;
			action.Description = description ?? action.Description;
			_context.UserActions.Update(action);
			_context.SaveChanges();
			return action;
		}
	}
}