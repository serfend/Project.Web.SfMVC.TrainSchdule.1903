using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService
	{
		public ApplyBaseInfo SubmitBaseInfo(ApplyBaseInfoDTO model)
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
		public async Task<ApplyBaseInfo> SubmitBaseInfoAsync(ApplyBaseInfoDTO model)
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
		public ApplyRequest SubmitRequest(ApplyRequestDTO model)
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
			return r;
		}
		public async  Task<ApplyRequest> SubmitRequestAsync(ApplyRequestDTO model)
		{
			return await Task.Run(()=> SubmitRequest(model));
		}
	}
}
