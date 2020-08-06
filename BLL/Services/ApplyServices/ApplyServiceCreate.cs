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
using DAL.Entities.Vacations;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService
	{
		private readonly IUsersService _usersService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IApplyAuditStreamServices _applyAuditStreamServices;
		private const string Const_LawVacationDescription = "法定节假日";

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
					Address = await _context.AdminDivisions.FindAsync(model.VacationTargetAddress).ConfigureAwait(true),
					AddressDetail = model.VacationTargetAddressDetail,
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
			var type = model.VacationType;
			if (type == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.VacationTypeNotExist);
			// 检查是否合法
			model.VacationAdditionals?.All(v =>
			{
				if (v.Description == Const_LawVacationDescription)
					throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.LawVacationCantCreateByUser);
				return true;
			});
			if (model.StampLeave != null)
			{
				var vacationLength = model.VacationLength;
				if (type.CanUseOnTrip) vacationLength += model.OnTripLength;
				// 仅计算正休假包含的法定节假日
				if (type.CaculateBenefit)
				{
					// 得到所有福利假后需要加入路途和原假并再次计算
					model.StampReturn = await vacationCheckServices.CrossVacation(model.StampLeave.Value, vacationLength, true).ConfigureAwait(true);
					List<VacationAdditional> lawVacations = vacationCheckServices.VacationDesc.Select(v => new VacationAdditional()
					{
						Name = v.Name,
						Start = v.Start,
						Length = v.Length,
						Description = Const_LawVacationDescription
					}).ToList();
					if (model.VacationAdditionals != null) lawVacations.AddRange(model.VacationAdditionals);
					model.VacationAdditionals = lawVacations;//执行完crossVacation后已经处于加载完毕状态可直接使用
					vacationLength += model.VacationAdditionals.Sum(v => v.Length);
				}
				model.StampReturn = model.StampLeave.Value.AddDays(vacationLength - 1);

				model.VacationDescriptions = vacationCheckServices.VacationDesc.CombineVacationDescription();
			}
			return model;
		}

		public async Task<ApplyRequest> SubmitRequestAsync(User targetUser, ApplyRequestVdto model)
		{
			if (model == null) return null;
			var vacationInfo = _usersService.VacationInfo(targetUser);
			if (vacationInfo.Description.Contains("无休假：")) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.ApplyMessage.Request.HaveNoVacationSinceExcept, vacationInfo.Description, true));
			model = await CaculateVacation(model).ConfigureAwait(true);
			var type = model.VacationType;
			if (type?.Disabled ?? true) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.InvalidVacationType);
			if (type.Primary)
			{
				// 剩余天数判断
				if (model.VacationLength > vacationInfo.LeftLength) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.ApplyMessage.Request.NoEnoughVacation, $"超出{model.VacationLength - vacationInfo.LeftLength}天", true));
				// 剩余路途次数判断
				if (model.OnTripLength < 0) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.Default);
			}
			// 不允许未休完正休假前休某些假
			else if (!type.AllowBeforePrimary && vacationInfo.LeftLength > 0)
				throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.ApplyMessage.Request.BeforePrimaryNotAllow, $"({type.Alias})", true));
			// 路途判断
			if (type.CanUseOnTrip)
			{
				if (model.OnTripLength > 0 && vacationInfo.MaxTripTimes <= vacationInfo.OnTripTimes)
					throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.TripTimesExceed);
			}
			else model.OnTripLength = 0;
			// 休假天数范围判断
			if (model.VacationLength < type.MinLength) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.VacationLengthTooShort);
			if (model.VacationLength > type.MaxLength) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.VacationLengthTooLong);
			// 福利假判断
			if (!type.CaculateBenefit)
				model.VacationAdditionals = null;
			// 跨年假判断
			if (type.NotPermitCrossYear)
				if (model.StampReturn.Value.Year != model.StampLeave.Value.Year) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.NotPermitCrossYear);

			var r = new ApplyRequest()
			{
				OnTripLength = model.OnTripLength,
				Reason = model.Reason,
				StampLeave = model.StampLeave,
				StampReturn = model.StampReturn,
				VacationLength = model.VacationLength,
				VacationPlace = model.VacationPlace,
				VacationPlaceName = model.VacationPlaceName,
				VacationType = model.VacationType.Name,
				CreateTime = DateTime.Now,
				ByTransportation = model.ByTransportation,
				AdditialVacations = model.VacationAdditionals,
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
			return Create(apply); // 创建成功，记录本次创建详情
		}

		public void InitAuditStream(Apply model)
		{
			var user = model?.BaseInfo?.From;
			if (user == null) return;
			var usrCmp = user.CompanyInfo.Company.Code;
			// 初始化审批流
			var rule = _applyAuditStreamServices.GetAuditSolutionRule(user, false);
			model.ApplyAuditStreamSolutionRule = rule ?? throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.AuditStreamMessage.StreamSolutionRule.NotExist);
			var modelApplyAllAuditStep = new List<ApplyAuditStep>();
			int stepIndex = 0;
			foreach (var nStr in (rule.Solution?.Nodes?.Length ?? 0) == 0 ? Array.Empty<string>() : rule.Solution?.Nodes?.Split("##"))
			{
				var n = _context.ApplyAuditStreamNodeActions.Where(a => a.Name == nStr).FirstOrDefault();
				if (n == null) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.Node.NotExist, $"无效的节点：{nStr}", true));

				// 当前单位设定为审批流最新节点的第一个符合条件的人，若此人不存在，则为上一节点的单位。
				if (modelApplyAllAuditStep.Count > 0)
				{
					var currentNodeFitMembers = modelApplyAllAuditStep[modelApplyAllAuditStep.Count - 1].MembersFitToAudit;
					var firstHandleUsr = (currentNodeFitMembers?.Length ?? 0) == 0 ? Array.Empty<string>() : currentNodeFitMembers.Split("##");
					if (firstHandleUsr != null && firstHandleUsr.Length > 0) usrCmp = _usersService.GetById(firstHandleUsr[0])?.CompanyInfo?.Company?.Code ?? usrCmp;
				}

				var nextNodeUsrCmp = usrCmp;
				var nextNodeFitMembers = string.Join("##", _applyAuditStreamServices.GetToAuditMembers(usrCmp, n.RegionOnCompany, n, true).ToList());
				var nextNodeFirstHandleUsr = (nextNodeFitMembers?.Length ?? 0) == 0 ? Array.Empty<string>() : nextNodeFitMembers.Split("##");
				if (nextNodeFirstHandleUsr != null && nextNodeFirstHandleUsr.Length > 0) nextNodeUsrCmp = _usersService.GetById(nextNodeFirstHandleUsr[0])?.CompanyInfo?.Company?.Code ?? usrCmp;

				var firstMemberCompany = _context.Companies.Where(c => c.Code == nextNodeUsrCmp).FirstOrDefault();
				var item = new ApplyAuditStep()
				{
					Index = stepIndex++,
					MembersAcceptToAudit = string.Empty,
					MembersFitToAudit = nextNodeFitMembers,
					RequireMembersAcceptCount = n.AuditMembersCount,
					Name = n.Name,
					FirstMemberCompanyName = firstMemberCompany?.Name,
					FirstMemberCompanyCode = firstMemberCompany?.Code
				};
				modelApplyAllAuditStep.Add(item);
			}
			model.ApplyAllAuditStep = modelApplyAllAuditStep;
			// 初始化审批记录，当每个人审批时，添加一条记录。并检查是否已全部完成审批，若已完成则进入下一步。
			model.Response = new List<ApplyResponse>();
		}

		/// <summary>
		/// 检查是否存在重复的时间范围的申请
		/// </summary>
		private void CheckIfHaveSameRangeVacation(Apply apply)
		{
			var r = apply.RequestInfo;
			var list = new List<AuditStatus>() {
					AuditStatus.Accept,
					AuditStatus.AcceptAndWaitAdmin,
					AuditStatus.Auditing
			};
			var userid = apply.BaseInfo.From.Id;
			var userVacationsInTime = _context.AppliesDb.Where(a => a.BaseInfo.From.Id == userid).Where(a =>
			(a.RequestInfo.StampLeave <= r.StampLeave && a.RequestInfo.StampReturn >= r.StampLeave) ||
			(a.RequestInfo.StampLeave <= r.StampReturn && a.RequestInfo.StampReturn >= r.StampReturn)
			).Where(a => list.Contains(a.Status));
			if (userVacationsInTime.Any()) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.CrashOtherVacation);
		}

		public void ModifyAuditStatus(Apply model, AuditStatus status, string authUser = null)
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
						CheckIfHaveSameRangeVacation(model);
						if (model.Status == AuditStatus.NotPublish || model.Status == AuditStatus.NotSave)
						{
							model.NowAuditStep = model.ApplyAllAuditStep.FirstOrDefault();
							model.Status = status;
							// 检查当前层级是否可审批
							GoNextStep(ref model, model.NowAuditStep, new List<ApplyAuditStep>(model.ApplyAllAuditStep), false);
							status = model.Status; // 接管状态
						}
						else throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.StatusInvalid.NotOnPublishable);

						break;//发布
					}
				case AuditStatus.Cancel:
					{
						if (model.Status == AuditStatus.Accept)
						{
							var lastLevel = model.ApplyAllAuditStep.LastOrDefault();
							if (lastLevel != null)
							{
								var userFit = lastLevel.MembersFitToAudit.Split("##");
								if (!userFit.Contains(authUser)) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.Cancel.CancelByNotSame);
							}
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
			var apply = model.Apply;
			if (apply == null) return ActionStatusMessage.ApplyMessage.NotExist;
			var nowStep = apply.NowAuditStep;
			List<ApplyAuditStep> allStep = new List<ApplyAuditStep>(apply.ApplyAllAuditStep);
			// 审批未发布时 不可进行审批
			if (nowStep == null) return ActionStatusMessage.ApplyMessage.Operation.Audit.BeenAuditOrNotReceived;
			if (apply.Status != AuditStatus.AcceptAndWaitAdmin && apply.Status != AuditStatus.Auditing) return ActionStatusMessage.ApplyMessage.Operation.Audit.NotOnAudingStatus;
			// 如果当前审批人是本单位管理，则本轮审批直接通过
			var company = nowStep.FirstMemberCompanyCode;
			var managers = companyManagerServices.GetManagers(company).Select(m => m.User.Id).ToList();
			var isManagerAudit = managers.Contains(AuditUser.Id);
			if (!isManagerAudit)
			{
				// 待审批人无当前用户时，返回无效
				if (!nowStep.MembersFitToAudit.Split("##").Contains(AuditUser.Id)) return ActionStatusMessage.ApplyMessage.Operation.Audit.NoYourAuditStream;
				if (nowStep.MembersAcceptToAudit.Split("##").Contains(AuditUser.Id)) return ActionStatusMessage.ApplyMessage.Operation.Audit.BeenAudit;
			}

			// 当审批的申请为未发布的申请时，将其发布
			//if (apply.Status == AuditStatus.NotSave || AuditStatus.NotPublish == apply.Status)
			//	ModifyAuditStatus(apply, AuditStatus.Auditing);
			var list = AddAuditRecord(nowStep, AuditUser, model);
			// 判断是否被驳回
			if (model.Action != AuditResult.Accept)
				apply.Status = AuditStatus.Denied;
			// 判断本步骤是否结束
			// 当  需审批人数<=已审批人数，需审批数为0表示需要所有人审批
			// 或  已审批人数=可审批人数
			// 或  为管理员审批
			else if (
				(nowStep.RequireMembersAcceptCount <= list.Length && nowStep.RequireMembersAcceptCount > 0)
				|| (nowStep.MembersAcceptToAudit.Length >= nowStep.MembersFitToAudit.Length)
				|| isManagerAudit)
			{
				GoNextStep(ref apply, nowStep, allStep);
			}
			_context.Applies.Update(apply);
			_context.SaveChanges();
			return ActionStatusMessage.Success;
		}

		private string[] AddAuditRecord(ApplyAuditStep nowStep, User AuditUser, ApplyAuditNodeVdto model)
		{
			var list = (nowStep.MembersAcceptToAudit?.Length ?? 0) == 0 ? Array.Empty<string>() : nowStep.MembersAcceptToAudit.Split("##");
			list = list.Append(AuditUser.Id).ToArray();
			nowStep.MembersAcceptToAudit = string.Join("##", list);
			_context.ApplyAuditSteps.Update(nowStep);
			// 对本人审批信息进行追加
			var responseList = new List<ApplyResponse>(model.Apply.Response);
			responseList.Add(new ApplyResponse()
			{
				// 通过判断有无授权码判断
				AuditingBy = AuditUser.Application.AuthKey != null ? AuditUser : null,
				Remark = model.Remark,
				HandleStamp = DateTime.Now,
				StepIndex = nowStep.Index,
				Status = model.Action == AuditResult.Accept ? Auditing.Accept : Auditing.Denied
			});
			model.Apply.Response = responseList;
			return list;
		}

		/// <summary>
		/// 检查当前步骤是否完成
		/// </summary>
		/// <param name="model"></param>
		/// <param name="nowStep"></param>
		/// <param name="allStep"></param>
		/// <param name="goNext">是否进行下一步，或者仅检查本级</param>
		private void GoNextStep(ref Apply model, ApplyAuditStep nowStep, List<ApplyAuditStep> allStep, bool goNext = true)
		{
			// 寻找下一个步骤
			if (nowStep.Index >= allStep.Count - 1 && goNext)
			{
				model.NowAuditStep = null;
				model.Status = AuditStatus.Accept;
			}
			else
			{
				if (goNext) model.NowAuditStep = allStep[nowStep.Index + 1];
				if (CheckStepShouldSkip(model.NowAuditStep))
				{
					AddAuditRecord(model.NowAuditStep, _usersService.GetById("audit_skipper"), new ApplyAuditNodeVdto()
					{
						Action = AuditResult.Accept,
						Remark = "无合适人可审,已跳过。",
						Apply = model
					});
					GoNextStep(ref model, model.NowAuditStep, allStep);
					return;
				}
				if (model.NowAuditStep.Index == allStep.Count - 1) model.Status = AuditStatus.AcceptAndWaitAdmin;
			}
		}

		/// <summary>
		/// 当前节点是否应该直接跳过
		/// </summary>
		/// <param name="nowStep"></param>
		/// <returns></returns>

		private static bool CheckStepShouldSkip(ApplyAuditStep nowStep) => nowStep.MembersFitToAudit.Length == 0;
	}
}