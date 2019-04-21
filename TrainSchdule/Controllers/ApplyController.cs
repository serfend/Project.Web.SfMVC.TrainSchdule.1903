﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Extensions;
using BLL.Interfaces;
using Castle.Core.Internal;
using DAL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TrainSchdule.BLL.Helpers;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.DAL.Entities;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Apply;

namespace TrainSchdule.Web.Controllers
{
	[Authorize]
	[Route("[controller]/[action]")]
	public class ApplyController: Controller
	{
		#region filed
		private readonly IUsersService _usersService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IApplyService _applyService;
		private readonly ICompaniesService _companiesService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IVerifyService _verifyService;

		private bool _isDisposed;

		public ApplyController(IUsersService usersService, ICurrentUserService currentUserService, IApplyService applyService, IUnitOfWork unitOfWork, ICompaniesService companiesService, IVerifyService verifyService)
		{
			_usersService = usersService;
			_currentUserService = currentUserService;
			_applyService = applyService;
			_unitOfWork = unitOfWork;
			_companiesService = companiesService;
			_verifyService = verifyService;
		}

		#endregion

		#region Logic

		//[HttpGet]
		//public IActionResult AllStatus()
		//{
		//	return new JsonResult(new ApplyAuditStatusViewModel()
		//	{
		//		Data = new ApplyAuditStatusData()
		//		{
		//			List = ApplyExtensions.StatusDic
		//		}
		//	});
		//}

		[HttpPost]
		public async Task<IActionResult> Submit([FromBody]ApplySubmitViewModel model)
		{
			//校验
			if(!ModelState.IsValid) return new JsonResult(new Status(ActionStatusMessage.Fail.status, JsonConvert.SerializeObject(ModelState.AllModelStateErrors())));
			var rst = new StringBuilder();
			if(! _verifyService.Verify(model.Verify))
				return new JsonResult(ActionStatusMessage.Account.Auth.Verify.Invalid);
			if(!_verifyService.Status.IsNullOrEmpty()) return new JsonResult(new Status(ActionStatusMessage.Account.Auth.Verify.Default.status, _verifyService.Status));
			var user = _currentUserService.CurrentUser;
			if (user == null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			if (user.Company==null) return new JsonResult(new Status(ActionStatusMessage.User.NoCompany.status, $"准备创建{user.RealName}({user.UserName})的申请，但此人无归属单位"));

			//初始化基础数据
			var item=new Apply()
			{
				Address = user.Address,
				Company = user.Company?.Path,
				Create = DateTime.Now,
				From = user,
				Status = AuditStatus.NotPublish,
				xjlb	= model.Param.xjlb,
				Request = model.Param.Request,

				Id=Guid.Empty,
				Response = null,
				stamp = null,
			};
			//初始化用户请求
			await _unitOfWork.ApplyRequests.CreateAsync(model.Param.Request);

			//初始化审核列表
			var responses=InitResponse(user,model.Param.To,rst);
			if(responses.Count==0)return new JsonResult(ActionStatusMessage.Apply.Operation.ToCompany.NoneToSubmit);
			item.Response = responses;
			item.stamp = model.Param.Stamp;
			await _unitOfWork.ApplyStamps.CreateAsync(item.stamp);

			var to=new List<Company>(item.Response.Count());
			foreach (var applyResponse in responses)
			{
				to.Add(applyResponse.Company);
			}
			item.To = to;
			var apply=await _applyService.CreateAsync(item);

			if (!model.NotAutoStart)
			{
				var startAudit=await StartAudit(apply.Id);
				var startAuditStatus = (Status) ((JsonResult) startAudit).Value;
				if (startAuditStatus.status != 0) return new JsonResult(new Status(startAuditStatus.status,$"申请创建成功,但未成功发布:{startAuditStatus.message}"));
			}
			return  new JsonResult(new ApplyCreatedViewModel()
			{
				Data = new ApplyCreatedDataModel()
				{
					Id = apply.Id
				}
			});
		}

		/// <summary>
		/// 初始化审核列表
		/// </summary>
		/// <param name="user">申请来源</param>
		/// <param name="To">申请发送至</param>
		/// <param name="rst">错误信息汇总</param>
		/// <returns></returns>
		private List<ApplyResponse> InitResponse(User user,IEnumerable<int>To,StringBuilder rst)
		{
			To = To.OrderBy(to => to);
			var list = new List<ApplyResponse>();
			var rawCompanyPath = user.Company?.Path;
			rawCompanyPath += '/';
			int lastIndex = 0;
			int nowSearchCount = -1;
			var anyFail = !To.All((index) =>
			{
				int chrIndex = 0;
				while (nowSearchCount < index)
				{
					chrIndex = rawCompanyPath.IndexOf('/', lastIndex);
					if (chrIndex == -1)
					{
						rst.AppendLine($"无效的待接收申请的单位:{index}");
						return false;
					}
					lastIndex = chrIndex + 1;
					nowSearchCount++;
				}
				var res = new ApplyResponse()
				{
					Status = Auditing.UnReceive,
					Company = _companiesService.GetCompanyByPath(rawCompanyPath.Substring(0, chrIndex)),
				};
				_unitOfWork.ApplyResponses.Create(res);
				list.Add(res);
				return true;
			});
			return list;
		}
		/// <summary>
		/// 对指定的申请开始处理流程
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult>StartAudit(Guid id)
		{
			var item=_unitOfWork.Applies.Get(id);
			if(item==null)return new JsonResult(ActionStatusMessage.Apply.NotExist);
			if (item.From.UserName != _currentUserService.CurrentUser.UserName)return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			if(item.Status!=AuditStatus.NotPublish)return new JsonResult(ActionStatusMessage.Apply.Operation.AuditBegan);


			var userActiveApplies = _applyService.GetAll(
				prevItem => 
					prevItem.From.UserName == item.From.UserName 
					&& (prevItem.Status == AuditStatus.Auditing 
					    || prevItem.Status == AuditStatus.Accept 
					    || prevItem.Status == AuditStatus.AcceptAndWaitAdmin)
				, 0,50);
			if (
				(from userActiveApply in userActiveApplies
					let prevStart = userActiveApply.Detail.Stamp.ldsj.Ticks
					let prevEnd = userActiveApply.Detail.Stamp.ldsj.AddDays(userActiveApply.Detail.Request.xjts).AddDays(userActiveApply.Detail.Request.ltts).Ticks
					let curStart = item.stamp.ldsj.Ticks let curEnd = item.stamp.ldsj.AddDays(item.Request.xjts).AddDays(item.Request.ltts).Ticks
					where (prevStart >= curStart && prevStart <= curEnd) || (prevEnd >= curStart && prevEnd <= curEnd)
					select prevStart).Any()
				)
			{
				return new JsonResult(ActionStatusMessage.Apply.Operation.AuditCrash);
			}


			item.Status = AuditStatus.Auditing;
			item.Response.Last().Status = Auditing.Received;
			_unitOfWork.Applies.Update(item);
			await _unitOfWork.SaveAsync();
			return new JsonResult(ActionStatusMessage.Success);
		}
		#endregion

		[HttpPost]
		public async Task<IActionResult> Withdraw(Guid id)
		{
			var item = _unitOfWork.Applies.Get(id);
			if (item == null) return new JsonResult(ActionStatusMessage.Apply.NotExist);
			if (item.From.UserName != _currentUserService.CurrentUser.UserName) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			if(item.Response.Any(apply=>apply.Status==Auditing.Accept))return new JsonResult(ActionStatusMessage.Apply.Operation.AuditBeenAcceptedByOneCompany);
			item.Status = AuditStatus.Withdrew;
			_unitOfWork.Applies.Update(item);
			await _unitOfWork.SaveAsync();
			return new JsonResult(ActionStatusMessage.Success);
		}

		[HttpDelete]
		public async Task<IActionResult> Remove(Guid id)
		{
			var item = _unitOfWork.Applies.Get(id);
			if (item == null) return new JsonResult(ActionStatusMessage.Apply.NotExist);
			if (item.From.UserName != _currentUserService.CurrentUser.UserName) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			switch (item.Status)
			{
				case AuditStatus.Accept:
					item.Hidden = true;
					_unitOfWork.Applies.Update(item);
					break;
				case AuditStatus.NotPublish:
					return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
					//_unitOfWork.Applies.Delete(id);
					//break;
				default:
					return new JsonResult(ActionStatusMessage.Apply.Operation.AuditIsPublic);
			}

			await _unitOfWork.SaveAsync();
			return new JsonResult(ActionStatusMessage.Success);
		}

		[HttpGet]
		public IActionResult FromCompany(string path,AuditStatus? status, int page = 0,int pageSize=10)
		{
			if (pageSize > 10) return new JsonResult(ActionStatusMessage.Apply.Operation.Invalid);

			var currentUser = _currentUserService.CurrentUser;
			if (path == null) path = currentUser.Company?.Path;
			if (path == null) return new JsonResult(new Status(ActionStatusMessage.User.NoCompany.status, $"检查当前用户{currentUser.RealName}({currentUser.UserName})的申请，但此人无归属单位"));

			var targetCompany = _companiesService.Get(path);
			if(targetCompany==null)return  new JsonResult(ActionStatusMessage.Company.NotExist);
			//因权限关系，用户不一定具有查看自己本单位申请的权限
			if (  !currentUser.PermissionCompanies.Any(cmp=>path.StartsWith(cmp.Path)))return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			Expression<Func<Apply, bool>> predict;
			if (status == null) predict = (item) => item.Response.Any(cmp => cmp.Company.Path == path);
			else predict=item=> item.Response.Any(cmp => cmp.Company.Path == path) && item.Status==status;
			var list = _applyService.GetAll(predict,page,pageSize);
			var summaryList= list.Select(applyAllDataDto => applyAllDataDto.ToSummaryDTO()).ToList();
			return new JsonResult(new ApplyProfileViewModel()
			{
				Data = new ApplyProfileDataModel(){Applies = summaryList}
			});
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult FromUser(string username,AuditStatus? status, int page = 0, int pageSize = 10)
		{
			if (pageSize > 10)return new JsonResult(ActionStatusMessage.Apply.Operation.Invalid);
			var currentUser = _currentUserService.CurrentUser;
			if (username == null) username = currentUser.UserName;
			if(username==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			var targetUser = _usersService.Get(username);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			var path = targetUser.Company?.Path;
			if(path==null)return new JsonResult(new Status(ActionStatusMessage.User.NoCompany.status, $"来自{targetUser.RealName}({targetUser.UserName})的申请，但此人无归属单位"));
			if (currentUser.UserName!=username 
			    && !currentUser.PermissionCompanies.Any(cmp => path.StartsWith(cmp.Path)))
				return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			Expression<Func<Apply, bool>> predict;
			if (status==null)
				 predict = (item) => item.From.UserName == targetUser.UserName;
			else
				predict = (item) => item.From.UserName == targetUser.UserName && status == item.Status;
			var list = _applyService.GetAll(predict, page, pageSize);
			var summaryList=list.Select(applyAllDataDto => applyAllDataDto.ToSummaryDTO());
			return new JsonResult(new ApplyProfileViewModel()
			{
				Data = new ApplyProfileDataModel() { Applies = summaryList }
			});
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Detail(Guid id)
		{
			var item = _applyService.Get(id);
			if(item==null)return new JsonResult(ActionStatusMessage.Apply.NotExist);
			var targetItemUser = _usersService.Get(item.FromUserName);
			if (targetItemUser == null) return new JsonResult(new Status(ActionStatusMessage.Apply.Default.status,$"申请来自:{item.From}({item.FromUserName})，但数据库中查无此人"));
			var targetItemCmp = targetItemUser.Company?.Path;
			if(targetItemCmp==null)return new JsonResult(new Status(ActionStatusMessage.Apply.Default.status,$"来自{item.From}({item.FromUserName})的申请，但此人无归属单位"));
			if (_currentUserService.CurrentUser.UserName==targetItemUser.UserName||_currentUserService.CurrentUser.PermissionCompanies.Any((cmp) => targetItemCmp.StartsWith(cmp.Path)))
			{
				return new JsonResult(new ApplyDetailViewModel()
				{
					Data = item
				});
			}else
				return  new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
		}

		[HttpPost]
		[AllowAnonymous]
		public async  Task<IActionResult>Auth([FromBody] IEnumerable<ApplyResponseHandleViewModel> Param)
		{
			if (!ModelState.IsValid) return new JsonResult(new Status(ActionStatusMessage.Fail.status, JsonConvert.SerializeObject(ModelState.AllModelStateErrors())));
			var errorList=new ConcurrentDictionary<string,List<Status>>();
			if(!User.Identity.IsAuthenticated)return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var currentUser = _currentUserService.CurrentUser;
			foreach (var applyAuth in Param)
			{
				errorList.AddOrUpdate(applyAuth.AuditAs, new List<Status>(),
					(key, statuses) => errorList[key] = statuses);
				//获取每个apply
				var item = _applyService.GetEntity(applyAuth.Id);
				if (item == null)
				{
					errorList[applyAuth.AuditAs].Add(ActionStatusMessage.Apply.NotExist);
					continue;
				}
				
				var auditCompany = _companiesService.GetCompanyByPath(applyAuth.AuditAs);
				if (auditCompany == null)
				{
					errorList[applyAuth.AuditAs].Add(ActionStatusMessage.Company.NotExist);
					continue;
				}
				bool havePermission =
					currentUser.PermissionCompanies.Any(cmp => auditCompany.Path.StartsWith(cmp.Path));
				if (!havePermission)
				{
					errorList[applyAuth.AuditAs].Add(ActionStatusMessage.Account.Auth.Invalid.Default);
					continue;
				};
				var proDTO = item.Response.Single(progress => progress.Company.Path == applyAuth.AuditAs);

				if (proDTO == null)
				{
					errorList[applyAuth.AuditAs].Add(ActionStatusMessage.Apply.Operation.ToCompany.NotExist);
					continue;
				}
				var pro = _unitOfWork.ApplyResponses.Get(proDTO.Id);
				if (pro.Status != Auditing.Received)
				{
					errorList[applyAuth.AuditAs].Add(ActionStatusMessage.Apply.Operation.Invalid);
					continue;
				}
				switch (applyAuth.Apply)
				{
					case ApplyReponseHandleStatus.Accept:
						pro.Status = Auditing.Accept;
						var nextProDTO = item.Response.TakeWhile(nextProgress => nextProgress.Status == Auditing.UnReceive)
							.LastOrDefault();
						if (nextProDTO == null)
						{
							item.Status = AuditStatus.AcceptAndWaitAdmin;
						}else						{
							var nextPro = _unitOfWork.ApplyResponses.Get(nextProDTO.Id);
							nextPro.Status = Auditing.Received;
						}
						break;
					case ApplyReponseHandleStatus.Deny:
						pro.Status = Auditing.Denied;
						item.Status = AuditStatus.Denied;
						
						break;
					default:
					{
						errorList[applyAuth.AuditAs].Add(ActionStatusMessage.Apply.Operation.Invalid);
						continue;
					}
				}
				
				pro.AuditingBy = currentUser;
				pro.Remark = applyAuth.Remark;
				pro.HandleStamp=DateTime.Now;
				_unitOfWork.Applies.Update(item);
				_unitOfWork.ApplyResponses.Update(pro);
				if (errorList[applyAuth.AuditAs].Count == 0) errorList.Remove(applyAuth.AuditAs,out var wasted);
			}

			await _unitOfWork.SaveAsync();
			return errorList.Count==0 ? new JsonResult(ActionStatusMessage.Success) : new JsonResult(new {code=-1,errors= errorList });
		}
		#region Disposing

		protected override void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing)
				{
					_usersService.Dispose();
					_currentUserService.Dispose();
				}

				_isDisposed = true;

				base.Dispose(disposing);
			}
		}

		#endregion
	}
}
