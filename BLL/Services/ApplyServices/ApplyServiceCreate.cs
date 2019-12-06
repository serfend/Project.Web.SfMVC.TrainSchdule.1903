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
		public ApplyBaseInfo SubmitBaseInfo(ApplyBaseInfoVdto model)
		{
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
			var m = new ApplyBaseInfo()
			{
				Company = await _context.Companies.FindAsync(model.Company),
				Duties = await _context.Duties.FirstOrDefaultAsync(d => d.Name == model.Duties),
				From = model.From,
				CreateBy=model.CreateBy,
				Social = new UserSocialInfo()
				{
					Address = await _context.AdminDivisions.FindAsync(model.VocationTargetAddress),
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
			await _context.ApplyBaseInfos.AddAsync(m);
			await _context.SaveChangesAsync();
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
				VocationType = model.VocationType,
				CreateTime = DateTime.Now,
				ByTransportation = model.ByTransportation,
				AdditialVocations=model.VocationAdditionals
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
				Status=AuditStatus.NotSave
			};
			if (apply.BaseInfo == null || apply.RequestInfo == null) return apply;
			var company = apply.BaseInfo?.Company;
			if (company == null) return apply;
			var user = apply.BaseInfo?.From;

			
			apply.Response = GetAuditStream(company,user);
			return Create(apply);
		}


		public IEnumerable<ApplyResponse> GetAuditStream(Company company,User ApplyUser)
		{
			var dutyType = _context.DutyTypes.Where(d => d.Duties.Code == ApplyUser.CompanyInfo.Duties.Code).FirstOrDefault();
			var auditStreamLength = dutyType?.AuditLevelNum == 0 ? (dutyType?.DutiesRawType == DutiesRawType.gb ? 3 : 2) : dutyType?.AuditLevelNum;
			var responses = new List<ApplyResponse>();
			var nowId = company?.Code;
			for(var i=0;i< auditStreamLength && nowId.Length>0; i++)//本级 上级
			{
				var t = GenerateAuditStream(nowId);
				if (t.Company != null) responses.Add(t);
				nowId = nowId.Substring(0, nowId.Length - 1);
			}
			responses.Add(GenerateAuditStream("ROOT"));//人力
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
			switch (status)
			{
				case AuditStatus.Withdrew:
					{
						if (model.Status == AuditStatus.Auditing)
						{
							_context.Applies.Update(model);
						}
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
						if (model.Status == AuditStatus.NotPublish||model.Status==AuditStatus.NotSave)
						{
							foreach (var r in model.Response)
							{
								r.Status = Auditing.Received;
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

		public IEnumerable<Status> Audit(ApplyAuditVdto model)
		{
			var myManages = _usersService.InMyManage(model.AuditUser.Id,out var totalCount)?.ToList();
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
			if (model.Apply == null) return ActionStatusMessage.Apply.NotExist;
			var nowAudit = new List<ApplyResponse>();
			foreach (var r in model.Apply.Response)
			{
				if (myManages.Any(c => c.Code == r.Company.Code))nowAudit.Add(r);
			}
			if (nowAudit.Count == 0) return ActionStatusMessage.Apply.Operation.Audit.NoYourAuditStream;
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
					if (next != null)
					{
						next.Status = Auditing.Received;//=下一级变更为审核中
						target.FinnalAuditCompany = next.Company.Code;
					}
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
