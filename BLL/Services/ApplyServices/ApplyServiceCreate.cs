using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.Vocations;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService
	{
		private readonly IUsersService _usersService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IApplyAuditStreamServices _applyAuditStreamServices;

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
					Settle = model.Settle // 此处可能需要静态化处理，但考虑到.History问题，再议
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

		public async Task<ApplyRequestVdto> CaculateVacation(ApplyRequestVdto model)
		{
			if (model == null) return null;
			bool CaculateAdditionalAndTripLength = model.VocationType == "正休";
			int additionalVocationDay = 0;
			model.VocationAdditionals?.All(v => { additionalVocationDay += v.Length; v.Start = DateTime.Now; return true; });
			if (model.StampLeave != null)
			{
				var vocationLength = model.VocationLength + (CaculateAdditionalAndTripLength ? (model.OnTripLength + additionalVocationDay) : 0);
				// 当未享受福利假时才计算法定节假日
				if (CaculateAdditionalAndTripLength && additionalVocationDay == 0)
				{
					model.StampReturn = await vocationCheckServices.CrossVocation(model.StampLeave.Value, vocationLength, CaculateAdditionalAndTripLength);
					List<VocationAdditional> lawVocations = vocationCheckServices.VocationDesc.Select(v => new VocationAdditional()
					{
						Name = v.Name,
						Start = v.Start,
						Length = v.Length,
						Description = "法定节假日"
					}).ToList();
					lawVocations.AddRange(model.VocationAdditionals);
					model.VocationAdditionals = lawVocations;//执行完crossVocation后已经处于加载完毕状态可直接使用
				}
				else model.StampReturn = model.StampLeave.Value.AddDays(vocationLength - 1);

				model.VocationDescriptions = vocationCheckServices.VocationDesc.CombineVocationDescription(CaculateAdditionalAndTripLength);
			}
			return model;
		}

		public async Task<ApplyRequest> SubmitRequestAsync(User targetUser, ApplyRequestVdto model)
		{
			if (model == null) return null;
			var vacationInfo = _usersService.VocationInfo(targetUser);
			model = await CaculateVacation(model).ConfigureAwait(true);
			switch (model.VocationType)
			{
				case "正休":
					if (model.OnTripLength > 0 && vacationInfo.MaxTripTimes <= vacationInfo.OnTripTimes) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.TripTimesExceed);
					if (model.VocationLength > vacationInfo.LeftLength) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.ApplyMessage.Request.NoEnoughVocation.Status, $"已无足够假期可以使用，超出{model.VocationLength - vacationInfo.LeftLength}天"));
					// TODO 改成可以自定义设置天数
					//if (model.VocationLength < 5) return new JsonResult(ActionStatusMessage.Apply.Request.VocationLengthTooShort);
					if (model.OnTripLength < 0) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.Default);
					break;

				case "事假":
					model.VocationAdditionals = null;
					model.OnTripLength = 0;
					break;

				case "病休":
					model.VocationAdditionals = null;
					model.OnTripLength = 0;
					break;

				default:
					throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.InvalidVocationType);
			}
			// TODO 改成可以自定义是否允许跨年
			//if (model.StampReturn.Value.Year != model.StampLeave.Value.Year) throw new ActionStatusMessageException(ActionStatusMessage.Apply.Request.NotPermitCrossYear);
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
				AdditialVocations = model.VocationAdditionals,
				VacationDescription = vacationInfo.Description
			};
			_context.ApplyRequests.Add(r);
			_context.SaveChanges();
			return r;
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
			if (rule == null) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.ApplyMessage.AuditStream.StreamSolutionRule.NotExist, "无合适规则", true));
			model.ApplyAuditStreamSolutionRule = rule;
			var modelApplyAllAuditStep = new List<ApplyAuditStep>();
			int stepIndex = 0;
			foreach (var nStr in (rule.Solution?.Nodes?.Length ?? 0) == 0 ? Array.Empty<string>() : rule.Solution?.Nodes?.Split("##"))
			{
				var n = _context.ApplyAuditStreamNodeActions.Where(a => a.Name == nStr).FirstOrDefault();
				if (n == null) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.ApplyMessage.AuditStream.Node.NotExist, $"无效的节点：{nStr}", true));

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
						if (model.Status == AuditStatus.Auditing || model.Status == AuditStatus.AcceptAndWaitAdmin)
						{
						}
						else if (model.Status == AuditStatus.Denied) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.Withdrew.AuditBeenDenied);
						else throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.StatusInvalid.NotOnAuditingStatus);
						break;//撤回
					}
				case AuditStatus.NotPublish:
					{
						if (model.Status == AuditStatus.NotSave)
						{
						}
						else throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.StatusInvalid.NotOnNotSaveStatus);
						break;//保存
					}
				case AuditStatus.Auditing:
					{
						if (model.Status == AuditStatus.NotPublish || model.Status == AuditStatus.NotSave)
						{
							model.NowAuditStep = model.ApplyAllAuditStep.FirstOrDefault();
						}
						else throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.StatusInvalid.NotOnPublishable);

						break;//发布
					}
				case AuditStatus.Cancel:
					{
						if (model.Status == AuditStatus.Accept)
						{
						}
						else throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.StatusInvalid.NotOnAccept);
						break; // 作废
					}
				default: throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.Invalid); ;//不支持其他
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
			if (model.Apply == null) return ActionStatusMessage.ApplyMessage.NotExist;
			var nowStep = model.Apply.NowAuditStep;
			List<ApplyAuditStep> allStep = new List<ApplyAuditStep>(model.Apply.ApplyAllAuditStep);
			// 审批未发布时 不可进行审批
			if (nowStep == null) return ActionStatusMessage.ApplyMessage.Operation.Audit.BeenAuditOrNotReceived;
			// 待审批人无当前用户时，返回无效
			if (!nowStep.MembersFitToAudit.Split("##").Contains(AuditUser.Id)) return ActionStatusMessage.ApplyMessage.Operation.Audit.NoYourAuditStream;
			if (nowStep.MembersAcceptToAudit.Split("##").Contains(AuditUser.Id)) return ActionStatusMessage.ApplyMessage.Operation.Audit.BeenAudit;
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
}