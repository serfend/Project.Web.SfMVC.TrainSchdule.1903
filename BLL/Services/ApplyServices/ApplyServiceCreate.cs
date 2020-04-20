using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService
	{
		private readonly IUsersService _usersService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IApplyAuditStreamServices _applyAuditStreamServices;

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
				Company = await _context.Companies.FindAsync(model.Company).ConfigureAwait(true),
				Duties = await _context.Duties.FirstOrDefaultAsync(d => d.Name == model.Duties).ConfigureAwait(true),
				From = model.From,
				CreateBy = model.CreateBy,
				Social = new UserSocialInfo()
				{
					Address = await _context.AdminDivisions.FindAsync(model.VocationTargetAddress).ConfigureAwait(true),
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
			await _context.ApplyBaseInfos.AddAsync(m).ConfigureAwait(true);
			await _context.SaveChangesAsync().ConfigureAwait(true);
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
			return await Task.Run(() => SubmitRequest(model)).ConfigureAwait(true);
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

			InitAuditStream(apply);
			return Create(apply);
		}

		public void InitAuditStream(Apply model)
		{
			var user = model?.BaseInfo?.From;
			if (user == null) return;
			var usrCmp = user.CompanyInfo.Company.Code;
			// 初始化审批流
			var rule = _applyAuditStreamServices.GetAuditSolutionRule(user);
			if (rule == null) throw new NoAuditStreamRuleFitException();
			model.ApplyAuditStreamSolutionRule = rule;
			var modelApplyAllAuditStep = new List<ApplyAuditStep>();
			int stepIndex = 0;
			foreach (var nStr in (rule.Solution?.Nodes?.Length ?? 0) == 0 ? Array.Empty<string>() : rule.Solution?.Nodes?.Split("##"))
			{
				var n = _context.ApplyAuditStreamNodeActions.Where(a => a.Name == nStr).FirstOrDefault();
				if (n == null) throw new NodeNotExistException($"无效的节点：{nStr}");

				// 当前单位设定为审批流最新节点的第一个符合条件的人，若此人不存在，则为上一节点的单位。
				if (modelApplyAllAuditStep.Count > 0)
				{
					var currentNodeFitMembers = modelApplyAllAuditStep[modelApplyAllAuditStep.Count - 1].MembersFitToAudit;
					var firstHandleUsr = (currentNodeFitMembers?.Length ?? 0) == 0 ? Array.Empty<string>() : currentNodeFitMembers.Split("##");
					if (firstHandleUsr != null && firstHandleUsr.Length > 0) usrCmp = _usersService.Get(firstHandleUsr[0])?.CompanyInfo?.Company?.Code ?? usrCmp;
				}

				var nextNodeUsrCmp = usrCmp;
				var nextNodeFitMembers = string.Join("##", _applyAuditStreamServices.GetToAuditMembers(usrCmp, n).ToList());
				var nextNodeFirstHandleUsr = (nextNodeFitMembers?.Length ?? 0) == 0 ? Array.Empty<string>() : nextNodeFitMembers.Split("##");
				if (nextNodeFirstHandleUsr != null && nextNodeFirstHandleUsr.Length > 0) nextNodeUsrCmp = _usersService.Get(nextNodeFirstHandleUsr[0])?.CompanyInfo?.Company?.Code ?? usrCmp;

				var item = new ApplyAuditStep()
				{
					Index = stepIndex++,
					MembersAcceptToAudit = string.Empty,
					MembersFitToAudit = nextNodeFitMembers,
					RequireMembersAcceptCount = n.AuditMembersCount,
					Name = n.Name,
					FirstMemberCompanyName = _context.Companies.Where(c => c.Code == nextNodeUsrCmp).FirstOrDefault()?.Name
				};
				modelApplyAllAuditStep.Add(item);
			}
			model.ApplyAllAuditStep = modelApplyAllAuditStep;

			// 初始化审批记录，当每个人审批时，添加一条记录。并检查是否已全部完成审批，若已完成则进入下一步。
			model.Response = new List<ApplyResponse>();
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
						}
						else if (model.Status == AuditStatus.Denied) throw new ActionStatusMessageException(ActionStatusMessage.Apply.Operation.Withdrew.AuditBeenDenied);
						else throw new ActionStatusMessageException(ActionStatusMessage.Apply.Operation.StatusInvalid.NotOnAuditingStatus);
						break;//撤回
					}
				case AuditStatus.NotPublish:
					{
						if (model.Status == AuditStatus.NotSave)
						{
						}
						else throw new ActionStatusMessageException(ActionStatusMessage.Apply.Operation.StatusInvalid.NotOnNotSaveStatus);
						break;//保存
					}
				case AuditStatus.Auditing:
					{
						if (model.Status == AuditStatus.NotPublish || model.Status == AuditStatus.NotSave)
						{
							model.NowAuditStep = model.ApplyAllAuditStep.FirstOrDefault();
						}
						else throw new ActionStatusMessageException(ActionStatusMessage.Apply.Operation.StatusInvalid.NotOnPublishable);

						break;//发布
					}
				default: throw new ActionStatusMessageException(ActionStatusMessage.Apply.Operation.Invalid); ;//不支持其他
			}

			model.Status = status;
			_context.Applies.Update(model);
			_context.SaveChanges();
		}

		public IEnumerable<ApiResult> Audit(ApplyAuditVdto model)
		{
			if (model == null) return null;

			var list = new List<ApiResult>();
			foreach (var apply in model.List)
			{
				var result = AuditSingle(apply, model.AuditUser);
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
		private ApiResult AuditSingle(ApplyAuditNodeVdto model, User AuditUser)
		{
			if (model.Apply == null) return ActionStatusMessage.Apply.NotExist;
			var nowStep = model.Apply.NowAuditStep;
			List<ApplyAuditStep> allStep = new List<ApplyAuditStep>(model.Apply.ApplyAllAuditStep);
			// 审批未发布时 不可进行审批
			if (nowStep == null) return ActionStatusMessage.Apply.Operation.Audit.BeenAuditOrNotReceived;
			// 待审批人无当前用户时，返回无效
			if (!nowStep.MembersFitToAudit.Split("##").Contains(AuditUser.Id)) return ActionStatusMessage.Apply.Operation.Audit.NoYourAuditStream;
			if (nowStep.MembersAcceptToAudit.Split("##").Contains(AuditUser.Id)) return ActionStatusMessage.Apply.Operation.Audit.BeenAudit;
			// 当审批的申请为未发布的申请时，将其发布
			//if (model.Apply.Status == AuditStatus.NotSave || AuditStatus.NotPublish == model.Apply.Status)
			//	ModifyAuditStatus(model.Apply, AuditStatus.Auditing);
			var list = (nowStep.MembersAcceptToAudit?.Length ?? 0) == 0 ? Array.Empty<string>() : nowStep.MembersAcceptToAudit.Split("##");
			list = list.Append(AuditUser.Id).ToArray();
			nowStep.MembersAcceptToAudit = string.Join("##", list);
			_context.ApplyAuditSteps.Update(nowStep);
			// 对本人审批信息进行追加
			var responseList = new List<ApplyResponse>(model.Apply.Response);
			responseList.Add(new ApplyResponse()
			{
				AuditingBy = AuditUser,
				Remark = model.Remark,
				HandleStamp = DateTime.Now,
				StepIndex = nowStep.Index,
				Status = model.Action == AuditResult.Accept ? Auditing.Accept : Auditing.Denied
			});
			model.Apply.Response = responseList;
			// 判断是否被驳回
			if (model.Action != AuditResult.Accept)
				model.Apply.Status = AuditStatus.Denied;
			// 判断本步骤是否结束    需审批人数<=已审批人数  或  已审批人数=可审批人数
			else if ((nowStep.RequireMembersAcceptCount <= list.Length && nowStep.RequireMembersAcceptCount > 0) || (nowStep.MembersAcceptToAudit.Length == nowStep.MembersFitToAudit.Length))
			{
				// 寻找下一个步骤
				if (nowStep.Index == allStep.Count - 1)
				{
					model.Apply.NowAuditStep = null;
					model.Apply.Status = AuditStatus.Accept;
				}
				else
				{
					model.Apply.NowAuditStep = allStep[nowStep.Index + 1];
					if (model.Apply.NowAuditStep.Index == allStep.Count - 1) model.Apply.Status = AuditStatus.AcceptAndWaitAdmin;
				}
			}
			_context.Applies.Update(model.Apply);
			_context.SaveChanges();
			return ActionStatusMessage.Success;
		}
	}

	[Serializable]
	public class NoAuditStreamRuleFitException : Exception
	{
		public NoAuditStreamRuleFitException() : base("没有合适的审批流方案")
		{
		}

		public NoAuditStreamRuleFitException(string message) : base(message)
		{
		}

		public NoAuditStreamRuleFitException(string message, Exception inner) : base(message, inner)
		{
		}

		protected NoAuditStreamRuleFitException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}

	[Serializable]
	public class NodeNotExistException : Exception
	{
		public NodeNotExistException() : base("节点未找到")
		{
		}

		public NodeNotExistException(string message) : base(message)
		{
		}

		public NodeNotExistException(string message, Exception inner) : base(message, inner)
		{
		}

		protected NodeNotExistException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}