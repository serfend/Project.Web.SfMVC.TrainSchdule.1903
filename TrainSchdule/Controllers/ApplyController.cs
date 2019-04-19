﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Extensions;
using BLL.Interfaces;
using Castle.Core.Internal;
using DAL.Entities;
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
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Submit([FromBody]ApplySubmitViewModel model)
		{
			//校验
			if(!ModelState.IsValid) return new JsonResult(new Status(ActionStatusMessage.AccountLogin_InvalidByUnknown.status, JsonConvert.SerializeObject(ModelState.AllModelStateErrors())));
			var rst = new StringBuilder();
			if(! _verifyService.Verify(model.Verify))
				return new JsonResult(ActionStatusMessage.AccountAuth_VerifyInvalid);
			if(!_verifyService.Status.IsNullOrEmpty()) return new JsonResult(new Status(ActionStatusMessage.AccountAuth_VerifyInvalid.status, _verifyService.Status));
			var user = _currentUserService.CurrentUser;
			if (user == null) return new JsonResult(ActionStatusMessage.AccountAuth_Invalid);
			if (user.Company==null) return new JsonResult(new Status(ActionStatusMessage.Apply_Unknow.status, $"准备创建{user.RealName}({user.UserName})的申请，但此人无归属单位"));

			//初始化基础数据
			var item=new Apply()
			{
				Address = user?.Address,
				Company = user?.Company?.Path,
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
			if(responses.Count==0)return new JsonResult(ActionStatusMessage.Apply_NoCompanyToSubmit);
			item.Response = responses;
			item.stamp = model.Param.Stamp;
			await _unitOfWork.ApplyStamps.CreateAsync(item.stamp);

			var to=new List<Company>(item.Response.Count());
			responses.All((res) =>
			{
				to.Add(res.Company);
				return true;
			});
			item.To = to;
			var apply=await _applyService.CreateAsync(item);

			if (!model.NotAutoStart)
			{
				var startAudit= StartAudit(apply.Id);
				var startAuditStatus = (Status) ((JsonResult) startAudit).Value;
				if (startAuditStatus.status != 0) return new JsonResult(new Status(startAuditStatus.status,$"申请创建成功,但未成功发布:{startAuditStatus.message}"));
			}
			return  new JsonResult(new ApplyCreatedViewModel()
			{
				Id = apply.Id
			});
		}

		/// <summary>
		/// 初始化审核列表
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		private List<ApplyResponse> InitResponse(User user,IEnumerable<int>To,StringBuilder rst)
		{
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
		[AllowAnonymous]
		public IActionResult StartAudit(Guid id)
		{
			var item=_unitOfWork.Applies.Get(id);
			if(item==null)return new JsonResult(ActionStatusMessage.Apply_NotExist);
			if (item.From.UserName != _currentUserService.CurrentUser.UserName)return new JsonResult(ActionStatusMessage.AccountAuth_Forbidden);
			if(item.Status!=AuditStatus.NotPublish)return new JsonResult(ActionStatusMessage.Apply_OperationAuditBegan);


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
				return new JsonResult(ActionStatusMessage.Apply_OperationAuditCrash);
			}


			item.Status = AuditStatus.Auditing;
			item.Response.First().Status = Auditing.Received;
			item.Response.All(res =>
			{
				_unitOfWork.ApplyResponses.Update(res);
				return true;
			});
			_unitOfWork.Applies.Update(item);
			_unitOfWork.Save();
			return new JsonResult(ActionStatusMessage.Success);
		}
		#endregion

		[HttpGet]
		[AllowAnonymous]
		public IActionResult FromCompany(string path = null, int page = 0,int pageSize=10)
		{
			if (path == null) path = _currentUserService.CurrentUser.Company?.Path;
			if (path == null) return new JsonResult(new Status(ActionStatusMessage.Apply_Unknow.status, $"检查当前用户{_currentUserService.CurrentUser.RealName}({_currentUserService.CurrentUser.UserName})的申请，但此人无归属单位"));

			var targetCompany = _companiesService.Get(path);
			if(targetCompany==null)return  new JsonResult(ActionStatusMessage.Company_NotExist);
			//因权限关系，用户不一定具有查看自己本单位申请的权限
			if (  !_currentUserService.CurrentUser.PermissionCompanies.Any(cmp=>path.StartsWith(cmp.Path)))return new JsonResult(ActionStatusMessage.AccountAuth_Forbidden);
			
			var list = _applyService.GetAll((item)=>item.To.Any(cmp=>cmp.Path==path),page,pageSize);
			var summaryList=new List<ApplyDTO>();
			list.All(item =>
			{
				summaryList.Add(item.ToSummaryDTO());
				return true;
			});
			return new JsonResult(new ApplyProfileViewModel()
			{
				Applies = summaryList
			});
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult FromUser(string username,AuditStatus? status, int page = 0, int pageSize = 10)
		{
			var currentUser = _currentUserService.CurrentUser;
			if (username == null) username = currentUser.UserName;
			var targetUser = _usersService.Get(username);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User_NotExist);
			var path = targetUser.Company?.Path;
			if(path==null)return new JsonResult(new Status(ActionStatusMessage.Apply_Unknow.status, $"来自{targetUser.RealName}({targetUser.UserName})的申请，但此人无归属单位"));
			if (_currentUserService.CurrentUser.UserName!=username && !_currentUserService.CurrentUser.PermissionCompanies.Any(cmp => path.StartsWith(cmp.Path))) return new JsonResult(ActionStatusMessage.AccountAuth_Forbidden);
			var list = _applyService.GetAll((item) => item.From.UserName==targetUser.UserName && status==item.Status, page, pageSize);
			var summaryList = new List<ApplyDTO>();
			list.All(item =>
			{
				summaryList.Add(item.ToSummaryDTO());
				return true;
			});
			return new JsonResult(new ApplyProfileViewModel()
			{
				Applies = summaryList
			});
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Detail(Guid id)
		{
			var item = _applyService.Get(id);
			if(item==null)return new JsonResult(ActionStatusMessage.Apply_NotExist);
			var targetItemUser = _usersService.Get(item.FromUserName);
			if (targetItemUser == null) return new JsonResult(new Status(ActionStatusMessage.Apply_Unknow.status,$"申请来自:{item.From}({item.FromUserName})，但数据库中查无此人"));
			var targetItemCmp = targetItemUser.Company?.Path;
			if(targetItemCmp==null)return new JsonResult(new Status(ActionStatusMessage.Apply_Unknow.status,$"来自{item.From}({item.FromUserName})的申请，但此人无归属单位"));
			if (_currentUserService.CurrentUser.UserName==targetItemUser.UserName||_currentUserService.CurrentUser.PermissionCompanies.Any((cmp) => targetItemCmp.StartsWith(cmp.Path)))
			{
				return new JsonResult(new ApplyDetailViewModel()
				{
					Data = item
				});
			}else
				return  new JsonResult(ActionStatusMessage.AccountLogin_InvalidAuth);
		}

		[HttpPost]
		[AllowAnonymous]
		public IActionResult Auth([FromBody] IEnumerable<ApplyResponseHandleViewModel> Param)
		{
			if (!ModelState.IsValid) return new JsonResult(new Status(ActionStatusMessage.AccountLogin_InvalidByUnknown.status, JsonConvert.SerializeObject(ModelState.AllModelStateErrors())));
			var errorlist=new List<Status>();
			if(!User.Identity.IsAuthenticated)return new JsonResult(ActionStatusMessage.AccountAuth_Invalid);
			var currentUser = _currentUserService.CurrentUser;
			foreach (var applyAuth in Param)
			{
				var item = _applyService.Get(applyAuth.Id);
				if (item == null) errorlist.Add(new Status(ActionStatusMessage.Apply_NotExist.status,applyAuth.Id.ToString()));
				else
				{
					var auditCompany = _companiesService.GetCompanyByPath(applyAuth.AuditAs);
					if(auditCompany==null)errorlist.Add(new Status(ActionStatusMessage.Company_NotExist.status,applyAuth.AuditAs));
					else
					{
						bool havePermission =
							currentUser.PermissionCompanies.Any(cmp => auditCompany.Path.StartsWith(cmp.Path));
						if(!havePermission)errorlist.Add(new Status(ActionStatusMessage.AccountAuth_Forbidden.status,applyAuth.AuditAs));
						else
						{
							var proDTO = item.Progress.TakeWhile(progress => progress.CompanyPath == applyAuth.AuditAs).FirstOrDefault();
							
							if (proDTO == null)errorlist.Add(new Status(ActionStatusMessage.Company_NotExist.status,applyAuth.AuditAs));
							var pro = _unitOfWork.ApplyResponses.Get(proDTO.Id);
							if(pro.Status!=Auditing.Received)return new JsonResult(ActionStatusMessage.Apply_OperationInvalid);
							switch (applyAuth.Apply)
							{
								case ApplyReponseHandleStatus.Accept:
									pro.Status = Auditing.Accept;
									var nextProDTO = item.Progress.TakeWhile(nextProgress => nextProgress.Status == Auditing.UnReceive)
										.FirstOrDefault();
									if (nextProDTO != null)
									{
										var nextPro = _unitOfWork.ApplyResponses.Get(nextProDTO.Id);
										nextPro.Status = Auditing.Received;
									}
									break;
								case ApplyReponseHandleStatus.Deny:
									pro.Status = Auditing.Denied;
									break;
							}

							pro.AuditingBy = currentUser;
							pro.Remark = applyAuth.Remark;
							pro.HandleStamp=DateTime.Now;
							_unitOfWork.ApplyResponses.Update(pro);
						}
					}
					
				}
			}
			_unitOfWork.Save();
			if(errorlist.Count==0)return new JsonResult(ActionStatusMessage.Success);
			return new JsonResult(new {code=-1,errors= errorlist });
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