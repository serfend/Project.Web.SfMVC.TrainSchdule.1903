using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService
	{
		public ApplyBaseInfo SubmitBaseInfo(ApplyBaseInfoVDTO model)
		{
			var m=new ApplyBaseInfo()
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
				}
			};
			_context.Add(m);
			_context.SaveChanges();
			return m;
		}
		public async Task<ApplyBaseInfo> SubmitBaseInfoAsync(ApplyBaseInfoVDTO model)
		{
			var m = new ApplyBaseInfo()
			{
				Company =await _context.Companies.FindAsync(model.Company),
				Duties =await _context.Duties.FindAsync(model.Duties),
				From = model.From,
				Social = new UserSocialInfo()
				{
					Address =await _context.AdminDivisions.FindAsync(model.HomeAddress),
					AddressDetail = model.HomeDetailAddress,
					Phone = model.Phone,
					Settle = model.Settle
				},
				RealName = model.RealName
			};
			await _context.ApplyBaseInfos.AddAsync(m);
			await _context.SaveChangesAsync();
			return m;
		}
		public ApplyRequest SubmitRequest(ApplyRequestVDTO model)
		{
			var r=new ApplyRequest()
			{
				OnTripLength = model.OnTripLength,
				Reason = model.Reason,
				StampLeave = model.StampLeave,
				StampReturn = model.StampReturn,
				VocationLength = model.VocationLength,
				VocationPlace = model.VocationPlace,
				VocationType = model.VocationType
			};
			_context.ApplyRequests.Add(r);
			_context.SaveChanges();
			return r;
		}
		public async  Task<ApplyRequest> SubmitRequestAsync(ApplyRequestVDTO model)
		{
			return await Task.Run(()=> SubmitRequest(model));
		}

		public Apply Submit(ApplyVDTO model)
		{
			var apply=new Apply()
			{
				BaseInfo = _context.ApplyBaseInfos.Find(model.BaseInfoId),
				Create = DateTime.Now,
				Hidden = false,
				RequestInfo = _context.ApplyRequests.Find(model.RequestInfoId),
				Status = AuditStatus.NotPublish
			};
			//流程 找到信息通信第二旅层级
			var company = apply.BaseInfo?.Company;
			if (company == null) return apply;

			
			apply.Response = GetAuditStream(company);
			
			return Create(apply);
		}

		public IEnumerable<ApplyResponse> GetAuditStream(Company company)
		{
			var responses = new List<ApplyResponse>();
			string nowId = company?.Code;
			bool anyToSubmit = false;
			while (nowId?.Length >= 7)
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
	}
}
