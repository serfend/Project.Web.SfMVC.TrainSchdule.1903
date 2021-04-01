using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO.Apply;
using DAL.DTO.Company;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.QueryModel;

namespace BLL.Interfaces
{
	public interface IApplyServiceManage<T,Q> where T:IAppliable where Q: IApplyRequestBase
	{
		/// <summary>
		/// 按筛选查询
		/// </summary>
		/// <param name="model"></param>
		/// <param name="getAllAppliesPermission">是否允许获取所有申请
		/// 若允许，将不再限制传入的日期是否合法</param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		IEnumerable<T> QueryApplies(QueryApplyDataModel model, bool getAllAppliesPermission, out int totalCount);


		/// <summary>
		/// 删除指定时间内未保存的申请
		/// </summary>
		/// <param name="interval"></param>
		/// <returns></returns>
		Task<int> RemoveAllUnSaveApply(TimeSpan interval);

		/// <summary>
		/// 移除已删除的用户所对应的申请
		/// </summary>
		/// <returns></returns>
		Task<int> RemoveAllRemovedUsersApply();
		/// <summary>
		/// 删除指定申请
		/// </summary>
		/// <param name="Applies"></param>
		Task RemoveApplies(IEnumerable<T> Applies);

		byte[] ExportExcel(string templete, ApplyDetailDto<Q> model);

		byte[] ExportExcel(string templete, IEnumerable<ApplyDetailDto<Q>> model);
	}
}