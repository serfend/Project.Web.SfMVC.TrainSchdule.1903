﻿using Abp.Extensions;
using BLL.Extensions;
using BLL.Extensions.CreateClientInfo;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Permission;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Common;
using DAL.Entities.Permisstions;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
		private readonly ApplicationDbContext context;
		private readonly IUserServiceDetail userServiceDetail;
		private readonly IUsersService usersService;
        private readonly IPermissionServices permissionServices;

        public UserActionServices(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, IUserServiceDetail userServiceDetail, IUsersService usersService, IPermissionServices permissionServices)
		{
			_httpContextAccessor = httpContextAccessor;
			this.context = context;
			this.userServiceDetail = userServiceDetail;
			this.usersService = usersService;
            this.permissionServices = permissionServices;
        }
		public UserAction Log(UserOperation operation, string username, string description, bool success = false, ActionRank rank = ActionRank.Debug)
		{
			var context = _httpContextAccessor.HttpContext;
			var ua = context.ClientInfo<UserAction>();
			ua.Date = DateTime.Now;
			ua.Operation = operation;
			ua.UserName = username;
			ua.Success = success;
			ua.Description = description;
			ua.Rank = rank;
			this.context.UserActions.Add(ua);
			this.context.SaveChanges();
			return ua;
		}

		public bool Permission(User authUser, DAL.Entities.Permisstions.Permission permission, PermissionType operation, string targetUserCompanyCode, string description)
        {
			if (authUser == null) return false;
			var authUserId = authUser.Id;
			var a = Log(UserOperation.Permission, authUserId, $"授权到{targetUserCompanyCode}执行{permission?.Key}@{operation} {description}", false, ActionRank.Danger);
			var permit = permissionServices.CheckPermissions(authUserId, permission.Key, operation, targetUserCompanyCode);
			if (permit != null)
			{
				Status(a, true, $"@直接权限:{permit.Name},{permit.Region},{permit.Type}");
				return true;
			}
			var checkCompanyMajor = authUser.CheckCompanyMajor(targetUserCompanyCode);
			if (checkCompanyMajor)
			{
				Status(a, true, $"单位主管");
				return true;
			}
			var checkCompanyManager = authUser.CheckCompanyManager(targetUserCompanyCode, userServiceDetail);
			if (checkCompanyManager)
			{
				Status(a, true, $"单位管理");
				return true;
			}
			return false;
		}
		public async Task<IEnumerable<UserAction>> Query(QueryUserActionViewModel model)
		{
			return await Task.Run(() =>
			{
				if (model == null) return null;
				var r = context.UserActionsDb.AsQueryable();
				if (model.UserName?.Value != null) r = r.Where(h => h.UserName == model.UserName.Value);
				if (model.Date?.Start != null && model.Date?.End != null)
				{
					r = r.Where(h => h.Date >= model.Date.Start).Where(h => h.Date <= model.Date.End);
				}
				if (model.Rank?.Arrays != null) r = r.Where(h => model.Rank.Arrays.Contains((int)h.Rank));
				if (model.Ip?.Arrays != null) r = r.Where(h => model.Ip.Arrays.Contains(h.Ip));
				if (model.Device?.Arrays != null) r = r.Where(h => model.Device.Arrays.Contains(h.Device));
				if (model.Message?.Value != null) r = r.Where(h => h.Description.Contains(model.Message.Value));
				if (model.Page == null) model.Page = new QueryByPage()
				{
					PageIndex = 0,
					PageSize = 20
				};
				return r.OrderByDescending(h => h.Date).Skip(model.Page.PageSize * model.Page.PageIndex).Take(model.Page.PageSize);
			}).ConfigureAwait(true);
		}

		public UserAction Status(UserAction action, bool success, string description = null)
		{
			if (action == null) return null;
			action.Success = success;
			if (description != null) description = $"$${description}";
			action.Description = $"{action.Description}{description}";
			context.UserActions.Update(action);
			context.SaveChanges();
			return action;
		}

		public ApiResult LogNewActionInfo(UserAction action, ApiResult message)
		{
			Status(action, message.Status == 0, message.Message.IsNullOrEmpty() ? null : message.Message);
			return message;
		}
	}
}