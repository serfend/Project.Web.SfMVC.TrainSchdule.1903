using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.BLL.Helpers;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.DAL.Entities;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.ViewModels.Apply;

namespace TrainSchdule.Web.Controllers
{
	[Route("[controller]/[action]")]
	public class ApplyController: Controller
	{
		#region filed
		private readonly IUsersService _usersService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IApplyService _applyService;
		private readonly ICompaniesService _companiesService;
		private readonly IUnitOfWork _unitOfWork;

		private bool _isDisposed;

		public ApplyController(IUsersService usersService, ICurrentUserService currentUserService, IApplyService applyService, IUnitOfWork unitOfWork)
		{
			_usersService = usersService;
			_currentUserService = currentUserService;
			_applyService = applyService;
			_unitOfWork = unitOfWork;
		}

		#endregion

		#region Logic
		[HttpPost]
		public async Task<IActionResult> Submit(ApplySubmitViewModel model)
		{
			var rst = new StringBuilder();
			var user = _currentUserService.CurrentUser;
			var item=new Apply()
			{
				Address = user.Address,
				Company = user.Company.Path,
				Create = DateTime.Now,
				From = user,
				Status = AuditStatus.NotPublish,
				xjlb	= model.Param.xjlb,
				Request = model.Param.Request,

				Id=Guid.Empty,
				Response = null,
				stamp = null,
				
			};
			await _unitOfWork.ApplyRequests.CreateAsync(model.Param.Request);
			var responses = new List<ApplyResponse>(model.Param.To.Count());
			var userCompanyPaths = user.Company.Path.Split('/');
			var anyFail=!model.Param.To.All( (index) => 
			{
				if (index < userCompanyPaths.Length)
				{
					var targetCompany = userCompanyPaths[index];

					var res = new ApplyResponse()
					{
						Status = Auditing.UnReceive,
						Company = _companiesService.GetCompanyByPath(targetCompany),
					};
					_unitOfWork.ApplyResponses.Create(res);
					responses.Add(res);
					
					return true;
				}
				else
				{
					rst.AppendLine($"无效的待接收申请的单位:{index}");
					return false;
				}

			});
			if(responses.Count==0)return new JsonResult(ActionStatusMessage.Apply_NoCompanyToSubmit);
			responses= responses.OrderBy((res) => res.Company.Path.Length).ToList();
			item.Response = responses;

			item.stamp = model.Param.Stamp;
			await _unitOfWork.ApplyStamps.CreateAsync(item.stamp);

			var to=new List<Company>(item.Response.Count());
			responses.All((res) =>
			{
				to.Add(res.Company);
				return true;
			});
			var apply=await _applyService.CreateAsync(item);
			await _unitOfWork.SaveAsync();
			if (!model.NotAutoStart) StartAudit(apply.Id);
			return  new JsonResult(new ApplyCreatedViewModel()
			{
				Id = apply.Id
			});
		}

		/// <summary>
		/// 对指定的申请开始处理流程
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult StartAudit(Guid id)
		{
			var item=_unitOfWork.Applies.Get(id);
			if(item==null)return new JsonResult(ActionStatusMessage.Apply_NotExist);
			if (item.From.UserName != _currentUserService.CurrentUser.UserName)return new JsonResult(ActionStatusMessage.AccountLogin_InvalidAuth);
			if(item.Status!=AuditStatus.NotPublish)return new JsonResult(ActionStatusMessage.Apply_OperationAuditBegan);
			item.Status = AuditStatus.Auditing;
			item.Response.First().Status = Auditing.Received;
			return new JsonResult(ActionStatusMessage.Success);
		}
		#endregion

		[HttpGet]
		public IActionResult FromCompany(string path = null, int page = 0,int pageSize=10)
		{
			if (path == null) path = _currentUserService.CurrentUser.Company.Path;
			if(!_currentUserService.CurrentUser.PermissionCompanies.Any(cmp=>path.StartsWith(cmp.Path)))return new JsonResult(ActionStatusMessage.AccountLogin_InvalidAuth);
			var list = _applyService.GetAll((item)=>item.To.Any(cmp=>cmp.Path==path),page,pageSize);
			return new JsonResult(new ApplyProfileViewModel()
			{
				Applies = list
			});
		}

		[HttpGet]
		public IActionResult FromUser(string username = null, int page = 0, int pageSize = 10)
		{
			if (username == null) username = _currentUserService.CurrentUser.UserName;
			var targetUser = _usersService.Get(username);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User_NotExist);
			var path = targetUser.Company.Path;
			if (!_currentUserService.CurrentUser.PermissionCompanies.Any(cmp => path.StartsWith(cmp.Path))) return new JsonResult(ActionStatusMessage.AccountLogin_InvalidAuth);
			var list = _applyService.GetAll((item) => item.From.UserName==targetUser.UserName, page, pageSize);
			return new JsonResult(new ApplyProfileViewModel()
			{
				Applies = list
			});
		}

		[HttpGet]
		public IActionResult Detail(Guid id)
		{
			var item = _applyService.Get(id);
			if(item==null)return new JsonResult(ActionStatusMessage.Apply_NotExist);
			var targetItemUser = _usersService.Get(item.FromUserName);
			if (targetItemUser == null) return new JsonResult(new Status(ActionStatusMessage.Apply_Unknow.Code,$"申请来自:{item.From}({item.FromUserName})，但数据库中查无此人"));
			var targetItemCmp = targetItemUser.Company?.Path;
			if(targetItemCmp==null)return new JsonResult(new Status(ActionStatusMessage.Apply_Unknow.Code,$"来自{item.From}({item.FromUserName})的申请，但此人无归属单位"));
			if (_currentUserService.CurrentUser.PermissionCompanies.Any((cmp) => targetItemCmp.StartsWith(cmp.Path)))
			{
				return new JsonResult(new ApplyDetailViewModel()
				{
					Data = item
				});
			}else
				return  new JsonResult(ActionStatusMessage.AccountLogin_InvalidAuth);
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
