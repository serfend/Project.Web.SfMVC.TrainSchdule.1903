using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService
	{
		private readonly IUsersService _usersService;
		public ApplyBaseInfo SubmitBaseInfo(ApplyBaseInfoVdto model)
		{
			var m = new ApplyBaseInfo()
			{
				Company = _context.Companies.Find(model.Company),
				Duties = _context.Duties.Find(model.Duties),
				From = model.From,
				Social = new UserSocialInfo()
				{
					Address = _context.AdminDivisions.Find(model.HomeAddress),
					AddressDetail = model.HomeDetailAddress,
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
			var m = new ApplyBaseInfo()
			{
				Company = await _context.Companies.FindAsync(model.Company),
				Duties = await _context.Duties.FirstOrDefaultAsync(d => d.Name == model.Duties),
				From = model.From,
				Social = new UserSocialInfo()
				{
					Address = await _context.AdminDivisions.FindAsync(model.HomeAddress),
					AddressDetail = model.HomeDetailAddress,
					Phone = model.Phone,
					Settle = model.Settle
				},
				RealName = model.RealName,
				CompanyName = model.Company,
				DutiesName = model.Duties
			};
			if (m.Company != null) m.CompanyName = m.Company.Name;
			await _context.ApplyBaseInfos.AddAsync(m);
			await _context.SaveChangesAsync();
			return m;
		}
		public ApplyRequest SubmitRequest(ApplyRequestVdto model)
		{
			var r = new ApplyRequest()
			{
				OnTripLength = model.OnTripLength,
				Reason = model.Reason,
				StampLeave = model.StampLeave,
				StampReturn = model.StampReturn,
				VocationLength = model.VocationLength,
				VocationPlace = model.VocationPlace,
				VocationType = model.VocationType,
				CreateTime = DateTime.Now
			};
			_context.ApplyRequests.Add(r);
			_context.SaveChanges();
			return r;
		}
		public async Task<ApplyRequest> SubmitRequestAsync(ApplyRequestVdto model)
		{
			return await Task.Run(() => SubmitRequest(model));
		}

		public Apply Submit(ApplyVdto model)
		{
			var apply = new Apply()
			{
				BaseInfo = _context.ApplyBaseInfos.Find(model.BaseInfoId),
				Create = DateTime.Now,
				Hidden = false,
				RequestInfo = _context.ApplyRequests.Find(model.RequestInfoId),
				//不要编辑当前状态，默认就是未保存
			};
			if (apply.BaseInfo == null || apply.RequestInfo == null) return apply;
			//流程 找到信息通信第二旅层级
			var company = apply.BaseInfo?.Company;
			if (company == null) return apply;

			apply.Response = GetAuditStream(company);
			return Create(apply);
		}

		public IEnumerable<Apply> GetApplyByToAuditCompany(string code)
		{
			return _context.Applies.Where(a => a.Response.Any(r => r.Company.Code == code));
		}

		public IEnumerable<Apply> GetApplyBySubmitCompany(string code)
		{
			return _context.Applies.Where(a => a.BaseInfo.Company.Code == code);
		}

		public IEnumerable<Apply> GetApplyBySubmitUser(string id)
		{
			return _context.Applies.Where(a => a.BaseInfo.From.Id == id);
		}


		public IEnumerable<ApplyResponse> GetAuditStream(Company company)
		{
			var responses = new List<ApplyResponse>();
			var nowId = company?.Code;
			var anyToSubmit = false;
			while (nowId?.Length >= 8)
			{
				var t = new ApplyResponse()
				{
					Status = Auditing.UnReceive,
					Company = _context.Companies.Find(nowId)
				};
				if (t.Company != null)
				{
					responses.Add(t);
					if (!anyToSubmit) anyToSubmit = true;
				}

				nowId = nowId.Substring(0, nowId.Length - 1);
			}

			return responses;
		}

		public bool ModifyAuditStatus(Apply model, AuditStatus status)
		{
			switch (status)
			{
				case AuditStatus.Withdrew:
					{
						if (model.Status == AuditStatus.Auditing)
						{
							_context.Applies.Update(model);
						}
						else return false;
						break;//撤回
					}
				case AuditStatus.NotPublish:
					{
						if (model.Status == AuditStatus.NotSave)
						{
							_context.Applies.Update(model);
						}
						else return false;
						break;//保存
					}
				case AuditStatus.Auditing:
					{
						if (model.Status == AuditStatus.NotPublish||model.Status==AuditStatus.NotSave)
						{
							foreach (var r in model.Response)
							{
								r.Status = Auditing.Received;
								break;
							}
							_context.Applies.Update(model);
						}
						else return false;

						break;//发布
					}
				default: return false;//不支持其他
			}

			model.Status = status;
			_context.SaveChanges();
			return true;
		}

		public IEnumerable<Status> Audit(ApplyAuditVdto model)
		{
			var myManages = _usersService.InMyManage(model.AuditUser.Id)?.ToList();
			if (myManages == null) throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.Default);

			var list =new List<Status>();
			foreach (var apply in model.List)
			{
				var result = AuditSingle(apply, myManages, model.AuditUser);
				list.Add(result);
			}
			
			_context.SaveChanges();
			return list;
		}

		private Status AuditSingle(ApplyAuditNodeVdto model, IEnumerable<Company> myManages,User AuditUser)
		{
			var nowAudit = new List<ApplyResponse>();
			foreach (var r in model.Apply.Response)
			{
				if (myManages.Any(c => c.Code == r.Company.Code))
				{
					nowAudit.Add(r);
					break;
				}
			}
			if (nowAudit.Count == 0) throw new ActionStatusMessageException(ActionStatusMessage.Apply.Operation.Audit.NoYourAuditStream);
			if (model.Apply.Status == AuditStatus.NotSave || AuditStatus.NotPublish == model.Apply.Status)
				ModifyAuditStatus(model.Apply, AuditStatus.Auditing);
			var result = AuditResponse(nowAudit, model.Apply, model.Action, model.Remark, AuditUser);
			return result;
		}
		private Status AuditResponse(IEnumerable<ApplyResponse> responses,Apply target,AuditResult action,string remark,User auditBy)
		{
			foreach (var r in responses)
			{
				if (r.Status == Auditing.Received)
				{
					r.Remark = remark;
					r.AuditingBy = auditBy;
					r.HandleStamp=DateTime.Now;
					AuditSingle(r,target,action);
					return ActionStatusMessage.Success;
				}
			}
			return ActionStatusMessage.Apply.Operation.Audit.BeenAuditOrNotReceived;
		}

		private void AuditSingle(ApplyResponse response, Apply target, AuditResult action)
		{
			switch (action)
			{
				case AuditResult.Accept:
					response.Status = Auditing.Accept;
					var next=target.Response.FirstOrDefault(r => r.Status == Auditing.UnReceive);
					if(next!=null)next.Status = Auditing.Received;//=下一级变更为审核中
					break;
				case AuditResult.Deny:
					response.Status = Auditing.Denied;
					target.Status = AuditStatus.Denied;
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
