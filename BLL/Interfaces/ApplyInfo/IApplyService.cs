using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.DTO.Apply;
using DAL.DTO.Company;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;

namespace BLL.Interfaces
{
	public interface IApplyService<T,Q> :  IApplyServiceManage<T,Q> where T: IAppliable where Q : IApplyRequestBase
	{
		T GetById(Guid id);
		/// <summary>
		/// 创建申请.
		/// </summary>
		T Create(T item);

		bool Edit(string id, Action<T> editCallBack);

		Task<bool> EditAsync(string id, Action<T> editCallBack);

		Task Delete(T item);

		T Submit(ApplyVdto model);
		IQueryable<T> CheckIfHaveSameRangeVacation(T apply);
	}
}