using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TrainSchdule.DAL.Entities;
using TrainSchdule.BLL.DTO;

namespace TrainSchdule.BLL.Interfaces
{
    /// <summary>
    /// 用户服务接口.
    /// 包含用户信息的相关处理.
    /// </summary>
    /// <remarks>
    /// 此接口需要反射调用.
    /// </remarks>
    public interface ICompaniesService : IDisposable
    {
	    Company GetCompanyByPath(string path);
		/// <summary>
		/// 加载所有单位的信息
		/// </summary>
		IEnumerable<CompanyDTO> GetAll(int page, int pageSize);

		IEnumerable<CompanyDTO> GetAll(Expression<Func<Company, bool>> predicate, int page, int pageSize);
		/// <summary>
		/// 通过单位路径
		/// </summary>
		CompanyDTO Get(string path);
		/// <summary>
		/// 找到所有子单位
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		IEnumerable<CompanyDTO> FindAllChild(Guid id);
        /// <summary>
        /// 设置单位的父单位
        /// </summary>
        void SetParent(Guid path,string ParentPath);

        /// <summary>
        /// 异步设置单位的父单位
        /// </summary>
        Task SetParentAsync(Guid id,string ParentPath);

       /// <summary>
	   /// 新增一个单位
	   /// </summary>
	   /// <param name="name">本单位的名称</param>
	   /// <returns></returns>
        Company Create(string name);

        /// <summary>
        /// 异步新增一个单位
        /// </summary>
        Task<Company> CreateAsync(string name);

        bool Edit(string path, Action<Company> editCallBack);

        Task<bool> EditAsync(string path, Action<Company> editCallBack);
    }
}
