using BLL.Extensions.Common;
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
		[Route("Social/UserModifyRecord")]
		[AllowAnonymous]
		public IActionResult SocialModifyRecord(string id)
		{
			var userid = id ?? currentUserService.CurrentUser?.Id;
			var u = usersService.GetById(id);
			if (u == null) return new JsonResult(id == null ? ActionStatusMessage.Account.Auth.Invalid.NotLogin : ActionStatusMessage.UserMessage.NotExist);
			var records = userServiceDetail.ModifyUserSettleModifyRecord(u);
			return new JsonResult(new SettleModifyRecordViewModel()
			{
				Data = new SettleModifyRecordDataModel()
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
		[Route("Social/ModifyRecord")]
		public IActionResult SingleSocialModifyRecord(int code)
		{
			var record = userServiceDetail.ModifySettleModifyRecord(code);
			if (record == null) return new JsonResult(ActionStatusMessage.StaticMessage.ResourceNotExist);
			return new JsonResult(new SingleSettleModifyRecordViewModel()
			{
				Data = new SingleSettleModifyRecordDataModel()
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
		[Route("Social/ModifyRecord")]
		public IActionResult SingleSocialModifyRecord([FromBody] ModifySingleSettleModifyRecordViewModel model)
		{
			var currentUser = currentUserService.CurrentUser;
			var authUser = model.Auth?.AuthUser(authService, usersService, currentUser?.Id);

			var newR = model.Record;
			var new_record_id = newR?.Code;
			if (new_record_id == null | new_record_id == 0) return new JsonResult(newR.NotExist());

			var targetUser = context.AppUsersDb.FirstOrDefault(u => u.SocialInfo.Settle.PrevYealyLengthHistory.FirstOrDefault(r => r.Code == new_record_id) != null);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var permit = userActionServices.Permission(authUser.Application.Permission, DictionaryAllPermission.User.SocialInfo, Operation.Update, authUser.Id, targetUser.CompanyInfo.CompanyCode);
			if (!permit) return new JsonResult(model.Auth.PermitDenied());
			var record = userServiceDetail.ModifySettleModifyRecord(newR.Code, (r) =>
			{
				if (r == null) return;
				r.IsNewYearInitData = newR.IsNewYearInitData;
				r.UpdateDate = newR.UpdateDate;
				r.Length = newR.Length;
				r.Description = newR.Description;
			}, model.Record.IsRemoved);
			if (record == null) return new JsonResult(record.NotExist());

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
			var targetUser = usersService.CurrentQueryUser(id);
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
		public IActionResult Settle([FromBody] UserSocialSettleModifyViewModel model)
		{
			var authUser = model?.Auth.AuthUser(authService, usersService, currentUserService.CurrentUser?.Id);
			var data = model?.Data;
			var userid = data?.Id;
			var user = usersService.GetById(userid);
			if (user == null) return new JsonResult(user.NotExist());
			if (data?.Settle == null) return new JsonResult(ActionStatusMessage.StaticMessage.ResourceNotExist);
			if (!userActionServices.Permission(authUser.Application.Permission, DictionaryAllPermission.User.SocialInfo, Operation.Update, authUser.Id, user.CompanyInfo.CompanyCode, $"修改{user.Id}")) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			user.SocialInfo.Settle = data.Settle.ToModel(context.AdminDivisions, user.SocialInfo.Settle);
			context.AppUserSocialInfoSettles.Update(user.SocialInfo.Settle);
			context.SaveChanges();
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}