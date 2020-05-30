using BLL.Helpers;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Extensions.Common;
using TrainSchdule.Extensions.Users.Social;
using TrainSchdule.ViewModels.User;
using TrainSchdule.ViewModels.User.Social;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers
{
	/// <summary>
	///
	/// </summary>
	public partial class UsersController
	{
		/// <summary>
		/// 获取用户的家庭变更情况
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("Social/UserModefyRecord")]
		[AllowAnonymous]
		public IActionResult SocialModefyRecord(string id)
		{
			var userid = id ?? _currentUserService.CurrentUser?.Id;
			var u = _usersService.Get(id);
			if (u == null) return new JsonResult(id == null ? ActionStatusMessage.Account.Auth.Invalid.NotLogin : ActionStatusMessage.UserMessage.NotExist);
			var records = userServiceDetail.ModefyUserSettleModefyRecord(u);
			return new JsonResult(new SettleModefyRecordViewModel()
			{
				Data = new SettleModefyRecordDataModel()
				{
					Records = records.OrderByDescending(r => r.UpdateDate)
				}
			});
		}

		/// <summary>
		/// 获取指定记录
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("Social/ModefyRecord")]
		public IActionResult SingleSocialModefyRecord(int code)
		{
			var record = userServiceDetail.ModefySettleModeyRecord(code);
			if (record == null) return new JsonResult(ActionStatusMessage.Static.ResourceNotExist);
			return new JsonResult(new SingleSettleModefyRecordViewModel()
			{
				Data = new SingleSettleModefyRecordDataModel()
				{
					Record = record
				}
			});
		}

		/// <summary>
		/// 修改指定记录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("Social/ModefyRecord")]
		public IActionResult SingleSocialModefyRecord([FromBody]ModefySingleSettleModefyRecordViewModel model)
		{
			var currentUser = _currentUserService.CurrentUser;
			if (!model.Auth.Verify(_authService, currentUser?.Id)) return new JsonResult(model.Auth.PermitDenied());
			var newR = model.Record;
			if (newR == null) return new JsonResult(newR.NotExist());
			var recod = userServiceDetail.ModefySettleModeyRecord(newR.Code, (r) =>
			{
				if (r == null) return;
				r.IsNewYearInitData = newR.IsNewYearInitData;
				r.UpdateDate = newR.UpdateDate;
				r.Length = newR.Length;
				r.Description = newR.Description;
			}, model.Record.IsRemoved);
			if (recod == null) return new JsonResult(recod.NotExist());
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 社会信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		[ProducesResponseType(typeof(UserSocialViewModel), 0)]
		[Route("[action]")]
		public IActionResult Social(string id)
		{
			var targetUser = GetCurrentQueryUser(id, out var result);
			if (targetUser == null) return result;
			return new JsonResult(new UserSocialViewModel()
			{
				Data = targetUser.SocialInfo.ToDto()
			});
		}

		/// <summary>
		/// 修改用户的家庭情况
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("social/[action]")]
		public IActionResult Settle([FromBody]UserSocialSettleModefyViewModel model)
		{
			var authUser = model?.Auth.AuthUser(_authService, _usersService, _currentUserService.CurrentUser?.Id);
			var data = model?.Data;
			var userid = data?.Id;
			var user = _usersService.Get(userid);
			if (user == null) return new JsonResult(user.NotExist());
			if (data?.Settle == null) return new JsonResult(ActionStatusMessage.Static.ResourceNotExist);
			if (!_userActionServices.Permission(authUser.Application.Permission, DictionaryAllPermission.User.SocialInfo, Operation.Update, authUser.Id, user.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			user.SocialInfo.Settle = data.Settle.ToModel(_context.AdminDivisions, user.SocialInfo.Settle);
			_context.AUserSocialInfoSettles.Update(user.SocialInfo.Settle);
			_context.SaveChanges();
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}