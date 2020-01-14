using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Duty;
using DAL.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService
	{
		private readonly IUsersService _usersService;
		private readonly ICurrentUserService _currentUserService;
		public ApplyBaseInfo SubmitBaseInfo(ApplyBaseInfoVdto model)
		{
			if (model == null) return null;
			var m = new ApplyBaseInfo()
			{
				Company = _context.Companies.Find(model.Company),
				Duties = _context.Duties.Find(model.Duties),
				From = model.From,
				Social = new UserSocialInfo()
				{
					Address = _context.AdminDivisions.Find(model.VocationTargetAddress),
					AddressDetail = model.VocationTargetAddressDetail,
					Phone = model.Phone,
					Settle = model.Settle
				},
				CreateTime = DateTime.Now
			};
			_context.Add(m);
			_context.SaveChanges();
			return m;
		}
		public async Task<ApplyBaseInfo> SubmitBaseInfoAsync(ApplyBaseInfoVdto model)
		{
			if (model == null) return null;
			var m = new ApplyBaseInfo()
			{
				Company = await _context.Companies.FindAsync(model.Company).ConfigureAwait(false),
				Duties = await _context.Duties.FirstOrDefaultAsync(d => d.Name == model.Duties).ConfigureAwait(false),
				From = model.From,
				CreateBy = model.CreateBy,
				Social = new UserSocialInfo()
				{
					Address = await _context.AdminDivisions.FindAsync(model.VocationTargetAddress).ConfigureAwait(false),
					AddressDetail = model.VocationTargetAddressDetail,
					Phone = model.Phone,
					Settle = model.Settle
				},
				RealName = model.RealName,
				CompanyName = model.Company,
				DutiesName = model.Duties,
				CreateTime = DateTime.Now
			};
			if (m.Company != null) m.CompanyName = m.Company.Name;
			await _context.ApplyBaseInfos.AddAsync(m).ConfigureAwait(false);
			await _context.SaveChangesAsync().ConfigureAwait(false);
			return m;
		}
		public ApplyRequest SubmitRequest(ApplyRequestVdto model)
		{
			if (model == null) return null;
			var r = new ApplyRequest()
			{
				OnTripLength = model.OnTripLength,
				Reason = model.Reason,
				StampLeave = model.StampLeave,
				StampReturn = model.StampReturn,
				VocationLength = model.VocationLength,
				VocationPlace = model.VocationPlace,
				VocationPlaceName = model.VocationPlaceName,
				VocationType = model.VocationType,
				CreateTime = DateTime.Now,
				ByTransportation = model.ByTransportation,
				AdditialVocations = model.VocationAdditionals
			};
			_context.ApplyRequests.Add(r);
			_context.SaveChanges();
			return r;
		}
		public async Task<ApplyRequest> SubmitRequestAsync(ApplyRequestVdto model)
		{
			return await Task.Run(() => SubmitRequest(model)).ConfigureAwait(false);
		}

		public Apply Submit(ApplyVdto model)
		{
			if (model == null) return null;
			var apply = new Apply()
			{
				BaseInfo = _context.ApplyBaseInfos.Find(model.BaseInfoId),
				Create = DateTime.Now,
				Hidden = false,
				RequestInfo = _context.ApplyRequests.Find(model.RequestInfoId),
				Status = AuditStatus.NotSave
			};
			if (apply.BaseInfo == null || apply.RequestInfo == null) return apply;
			var company = apply.BaseInfo?.Company;
			if (company == null) return apply;
			var user = apply.BaseInfo?.From;


			apply.Response = GetAuditStream(company, user);
			return Create(apply);
		}


		public IEnumerable<ApplyResponse> GetAuditStream(Company company, User ApplyUser)
		{
			var dutyType = _context.DutyTypes.Where(d => d.Duties.Code == ApplyUser.CompanyInfo.Duties.Code).FirstOrDefault();
			int auditStreamLength = 0;
			auditStreamLength = dutyType == null ? 3 : dutyType.AuditLevelNum;//当没有职务类别时，默认需要三个层级进行审批
			var responses = new List<ApplyResponse>();
			var nowId = company?.Code;
			for (var i = 0; i < auditStreamLength && nowId.Length > 0; i++)//本级 上级
			{
				var t = GenerateAuditStream(nowId);
				if (t.Company != null) responses.Add(t);
				nowId = nowId.Substring(0, nowId.Length - 1);
			}
			//responses.Add(GenerateAuditStream("ROOT"));//人力
			//无需人力终审，人力同时执行A层级
			//type1人员 AAA层级和AA层级  type2人员AAAA、AAA、AA层级即可
			return responses;
		}
		public ApplyResponse GenerateAuditStream(string companyId)
		{
			return new ApplyResponse()
			{
				Status = Auditing.UnReceive,
				Company = _context.Companies.Find(companyId)
			};
		}
		public void ModifyAuditStatus(Apply model, AuditStatus status)
		{
			if (model == null) return;
			switch (status)
			{
				case AuditStatus.Withdrew:
					{
						if (model.Status == AuditStatus.Auditing)
						{
							if (model.Response.Any(r => r.Status == Auditing.Accept)) throw new ActionStatusMessageException(ActionStatusMessage.Apply.Operation.Withdrew.AuditBeenAcceptedByOneCompany);
							_context.Applies.Update(model);
						}
						else if (model.Status == AuditStatus.Denied) throw new ActionStatusMessageException(ActionStatusMessage.Apply.Operation.Withdrew.AuditBeenDenied);
						else throw new ActionStatusMessageException(ActionStatusMessage.Apply.Operation.StatusInvalid.NotOnAuditingStatus);
						break;//撤回
					}
				case AuditStatus.NotPublish:
					{
						if (model.Status == AuditStatus.NotSave)
						{
							_context.Applies.Update(model);
						}
						else throw new ActionStatusMessageException(ActionStatusMessage.Apply.Operation.StatusInvalid.NotOnNotSaveStatus);
						break;//保存
					}
				case AuditStatus.Auditing:
					{
						if (model.Status == AuditStatus.NotPublish || model.Status == AuditStatus.NotSave)
						{
							foreach (var r in model.Response)
							{
								r.Status = Auditing.Received;
								model.NowAuditCompany = r.Company.Code;
								model.NowAuditCompanyName = r.Company.Name;
								break;
							}
							_context.Applies.Update(model);
						}
						else throw new ActionStatusMessageException(ActionStatusMessage.Apply.Operation.StatusInvalid.NotOnPublishable);

						break;//发布
					}
				default: throw new ActionStatusMessageException(ActionStatusMessage.Apply.Operation.Invalid); ;//不支持其他
			}

			model.Status = status;
			_context.SaveChanges();
		}

		public IEnumerable<ApiResult> Audit(ApplyAuditVdto model)
		{
			if (model == null) return null;
			// 获取授权用户的所有管辖单位
			var myManages = _usersService.InMyManage(model.AuditUser, out var totalCount)?.ToList();

			if (myManages == null) throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.Default);

			var list = new List<ApiResult>();
			foreach (var apply in model.List)
			{
				var result = AuditSingle(apply, myManages, model.AuditUser);
				list.Add(result);
			}

			_context.SaveChanges();
			return list;
		}
		/// <summary>
		/// 用户对指定申请进行审批
		/// </summary>
		/// <param name="model">目标申请</param>
		/// <param name="myManages">用户所管辖的单位列表</param>
		/// <param name="AuditUser">审批人</param>
		/// <returns></returns>
		private ApiResult AuditSingle(ApplyAuditNodeVdto model, IEnumerable<Company> myManages, User AuditUser)
		{
			if (model.Apply == null) return ActionStatusMessage.Apply.NotExist;
			var nowAudit = new List<ApplyResponse>();//获取所有可能审批的流程
			foreach (var r in model.Apply.Response) if (myManages.Any(c => c.Code == r.Company.Code)) nowAudit.Add(r);
			if (nowAudit.Count == 0) return ActionStatusMessage.Apply.Operation.Audit.NoYourAuditStream;

			// 当审批的申请为未发布的申请时，将其发布
			if (model.Apply.Status == AuditStatus.NotSave || AuditStatus.NotPublish == model.Apply.Status)
				ModifyAuditStatus(model.Apply, AuditStatus.Auditing);

			var result = AuditResponse(nowAudit, model.Apply, model.Action, model.Remark, AuditUser);
			return result;
		}
		/// <summary>
		/// 对一个审批流程进行审批
		/// </summary>
		/// <param name="responses">目标审批流程</param>
		/// <param name="target">目标申请</param>
		/// <param name="action">审批操作</param>
		/// <param name="remark">备注</param>
		/// <param name="auditBy">审批人</param>
		/// <returns></returns>
		private ApiResult AuditResponse(IEnumerable<ApplyResponse> responses, Apply target, AuditResult action, string remark, User auditBy)
		{
			foreach (var r in responses)
			{
				// 审批流程中第一个正在审批中的流程
				if (r.Status == Auditing.Received)
				{
					r.Remark = remark;
					r.AuditingBy = auditBy;
					r.HandleStamp = DateTime.Now;
					AuditSingle(r, target, action);
					return ActionStatusMessage.Success;
				}
			}
			return ActionStatusMessage.Apply.Operation.Audit.BeenAuditOrNotReceived;
		}
		/// <summary>
		/// 对单个流程进行审批
		/// </summary>
		/// <param name="response">目标审批流程</param>
		/// <param name="target">目标申请</param>
		/// <param name="action">审批操作</param>
		private static void AuditSingle(ApplyResponse response, Apply target, AuditResult action)
		{
			switch (action)
			{
				case AuditResult.Accept:
					response.Status = Auditing.Accept;
					var next = target.Response.FirstOrDefault(r => r.Status == Auditing.UnReceive);
					if (next != null)
					{
						next.Status = Auditing.Received;// 下一级变更为审核中
						target.FinnalAuditCompany = next.Company.Code;
						target.NowAuditCompany = target.FinnalAuditCompany;
						target.NowAuditCompanyName = next.Company.Name;
					}
					break;
				case AuditResult.Deny:
					response.Status = Auditing.Denied;
					target.Status = AuditStatus.Denied;
					target.NowAuditCompany = response.Company.Code;
					target.NowAuditCompanyName = response.Company.Name;
					return;
				case AuditResult.NoAction:
					return;
			}
			switch (target.Response.Count(r => r.Status != Auditing.Accept))
			{
				case 1:
					target.Status = AuditStatus.AcceptAndWaitAdmin;
					break;
				case 0:
					target.Status = AuditStatus.Accept;
					break;
			}
		}
	}
}
