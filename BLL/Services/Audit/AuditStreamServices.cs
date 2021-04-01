using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Audit;
using DAL.Data;
using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Audit
{
   public  class AuditStreamServices:IAuditStreamServices
    {
        private readonly IUsersService usersService;
        private readonly ICompanyManagerServices companyManagerServices;
        private readonly ApplicationDbContext context;
        private readonly IApplyAuditStreamServices applyAuditStreamServices;

        public AuditStreamServices(IUsersService usersService, ICompanyManagerServices companyManagerServices,ApplicationDbContext context, IApplyAuditStreamServices applyAuditStreamServices)
        {
            this.usersService = usersService;
            this.companyManagerServices = companyManagerServices;
            this.context = context;
            this.applyAuditStreamServices = applyAuditStreamServices;
        }
        public void ModifyAuditStatus<T>(ref T model, AuditStatus status, string authUser = null)where T:IAuditable
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
		}

		public void InitAuditStream<T>(ref T model,string entityType,User user) where T : IAuditable
		{
			if (user == null) return;
			var usrCmp = user.CompanyInfo.CompanyCode;
			// 初始化审批流
			var rule = applyAuditStreamServices.GetAuditSolutionRule(user, entityType, false);
			model.ApplyAuditStreamSolutionRule = rule ?? throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.AuditStreamMessage.StreamSolutionRule.NotExist);
			var modelApplyAllAuditStep = new List<ApplyAuditStep>();
			int stepIndex = 0;
			foreach (var nStr in (rule.Solution?.Nodes?.Length ?? 0) == 0 ? Array.Empty<string>() : rule.Solution?.Nodes?.Split("##"))
			{
				var n = context.ApplyAuditStreamNodeActionDb.Where(a => a.Name == nStr).FirstOrDefault();
				if (n == null) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.ApplyMessage.AuditStreamMessage.Node.NotExist, $"无效的节点：{nStr}", true));

				// 当前单位设定为审批流最新节点的第一个符合条件的人，若此人不存在，则为上一节点的单位。
				if (modelApplyAllAuditStep.Count > 0)
				{
					var currentNodeFitMembers = modelApplyAllAuditStep[modelApplyAllAuditStep.Count - 1].MembersFitToAudit;
					var firstHandleUsr = (currentNodeFitMembers?.Length ?? 0) == 0 ? Array.Empty<string>() : currentNodeFitMembers.Split("##");
					if (firstHandleUsr != null && firstHandleUsr.Length > 0) usrCmp = usersService.GetById(firstHandleUsr[0])?.CompanyInfo?.CompanyCode ?? usrCmp;
				}

				var nextNodeUsrCmp = usrCmp;
				var nextNodeFitMembers = string.Join("##", applyAuditStreamServices.GetToAuditMembers(usrCmp, n.RegionOnCompany, n, true).ToList());
				var nextNodeFirstHandleUsr = (nextNodeFitMembers?.Length ?? 0) == 0 ? Array.Empty<string>() : nextNodeFitMembers.Split("##");
				if (nextNodeFirstHandleUsr != null && nextNodeFirstHandleUsr.Length > 0) nextNodeUsrCmp = usersService.GetById(nextNodeFirstHandleUsr[0])?.CompanyInfo?.CompanyCode ?? usrCmp;

				var firstMemberCompany = context.CompaniesDb.FirstOrDefault(c => c.Code == nextNodeUsrCmp);
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

		public IEnumerable<ApiResult> Audit<T>(ref ApplyAuditVdto<T> model)where T:IAuditable
		{
			if (model == null) return null;

			var list = new List<ApiResult>();
			var handles = model.List.ToList();
			for (var i=0;i< handles.Count;i++)
			{
				var apply = handles[i];
				var result = AuditSingle(ref apply, model.AuditUser);
				list.Add(result);
			}
			model.List = handles;
			return list;
		}

		/// <summary>
		/// 用户对指定申请进行审批
		/// </summary>
		/// <param name="model">目标申请</param>
		/// <param name="myManages">用户所管辖的单位列表</param>
		/// <param name="AuditUser">审批人</param>
		/// <returns></returns>
		private ApiResult AuditSingle<T>(ref ApplyAuditNodeVdto<T> model, User AuditUser)where T:IAuditable
		{
			var apply = model.AuditItem;
			if (apply == null) return ActionStatusMessage.ApplyMessage.NotExist;
			var nowStep = apply.NowAuditStep;
			List<ApplyAuditStep> allStep = new List<ApplyAuditStep>(apply.ApplyAllAuditStep);
			// 审批未发布时 不可进行审批
			if (nowStep == null) return ActionStatusMessage.ApplyMessage.Operation.Audit.BeenAuditOrNotReceived;
			if (apply.Status != AuditStatus.AcceptAndWaitAdmin && apply.Status != AuditStatus.Auditing) return ActionStatusMessage.ApplyMessage.Operation.Audit.NotOnAudingStatus;
			// 如果当前审批人是本单位管理，则本轮审批直接通过
			var company = nowStep.FirstMemberCompanyCode;
			var managers = companyManagerServices.GetManagers(company).Select(m => m.UserId).ToList();
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
			return ActionStatusMessage.Success;
		}

		private string[] AddAuditRecord<T>(ApplyAuditStep nowStep, User AuditUser, ApplyAuditNodeVdto<T> model)where T:IAuditable
		{
			var list = (nowStep.MembersAcceptToAudit?.Length ?? 0) == 0 ? Array.Empty<string>() : nowStep.MembersAcceptToAudit.Split("##");
			list = list.Append(AuditUser.Id).ToArray();
			nowStep.MembersAcceptToAudit = string.Join("##", list);
			context.ApplyAuditSteps.Update(nowStep);
			// 对本人审批信息进行追加
			var responseList = new List<ApplyResponse>(model.AuditItem.Response);
			responseList.Add(new ApplyResponse()
			{
				// 通过判断有无授权码判断
				AuditingBy = AuditUser.Application.AuthKey != null ? AuditUser : null,
				Remark = model.Remark,
				HandleStamp = DateTime.Now,
				StepIndex = nowStep.Index,
				Status = model.Action == AuditResult.Accept ? Auditing.Accept : Auditing.Denied
			});
			model.AuditItem.Response = responseList;
			return list;
		}

		/// <summary>
		/// 检查当前步骤是否完成
		/// </summary>
		/// <param name="model"></param>
		/// <param name="nowStep"></param>
		/// <param name="allStep"></param>
		/// <param name="goNext">是否进行下一步，或者仅检查本级</param>
		private void GoNextStep<T>(ref T model, ApplyAuditStep nowStep, List<ApplyAuditStep> allStep, bool goNext = true)where T: IAuditable
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
					AddAuditRecord(model.NowAuditStep, usersService.GetById("audit_skipper"), new ApplyAuditNodeVdto<T>()
					{
						Action = AuditResult.Accept,
						Remark = "无合适人可审,已跳过。",
						AuditItem = model
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
