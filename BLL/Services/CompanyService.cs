using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TrainSchdule.BLL.DTO;
using TrainSchdule.BLL.Extensions;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.DAL.Entities;
using TrainSchdule.DAL.Interfaces;

namespace TrainSchdule.BLL.Services
{
	public class CompanyService:ICompanyService
	{
		
		private readonly IUnitOfWork _unitOfWork;
		
		private readonly IHttpContextAccessor _httpContextAccessor;
		#region Disposing
		private bool _isDisposed;

		public CompanyService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
		{
			_unitOfWork = unitOfWork;
			_httpContextAccessor = httpContextAccessor;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing)
				{
					_unitOfWork.Dispose();
				}

				_isDisposed = true;
			}
		}

		~CompanyService()
		{
			Dispose(false);
		}

		#endregion
		private Company GetCompanyByPath(string path)
		{
			var company =  _unitOfWork.Companys.Find(x => x.Path == path).FirstOrDefault();
			return company;
		}
		public IEnumerable<CompanyDTO> GetAll(int page, int pageSize)
		{
			var companys= _unitOfWork.Companys.GetAll(page, pageSize);
			var result = new List<CompanyDTO>(companys.Count());
			foreach (var company in companys)
			{
				result.Add(MapCompany(company));
			}

			return result;
		}
		public CompanyDTO Get(string path)
		{
			var company = GetCompanyByPath(path);
			return MapCompany(company);
		}

		public IEnumerable<CompanyDTO> FindAllChild(string path)
		{
			var list = _unitOfWork.Companys.Find(x => x.Parent.Path == path);
			var result=new List<CompanyDTO>(list.Count());
			foreach (var company in list)
			{
				result.Add(MapCompany(company));
			}

			return result;
		}

		public void SetParent(string path, string parentPath)
		{
			var company = GetCompanyByPath(path);
			var parentCompany = GetCompanyByPath(parentPath);
			if (company != null && parentCompany!=null)
			{
				company.Parent = parentCompany;
				company.Path = $"{parentCompany.Path}/{company.Name}";
				_unitOfWork.Save();
			}
		}

		public async Task SetParentAsync(string path, string parentPath)
		{
			var company = GetCompanyByPath(path);
			var parentCompany = GetCompanyByPath(parentPath);
			if (company != null && parentCompany!=null)
			{
				company.Parent = parentCompany;
				company.Path = $"{parentCompany.Path}/{company.Name}";
				await _unitOfWork.SaveAsync();
			}
		}


		public Company Create(string name, string node = "root")
		{
			var company=new Company()
			{
				Name = name,
				Path = $"{node}/{name}"
			};
			_unitOfWork.Companys.Create(company);
			return company;
		}

		public async Task<Company> CreateAsync(string name, string node = "root")
		{
			var company=new Company()
			{
				Name = name,
				Path = $"{node}/{name}"
			};
			await  _unitOfWork.Companys.CreateAsync(company);
			return company;
		}


		#region Helpers

		/// <summary>
		/// Helps map user entity to user data transfer object.
		/// </summary>
		protected CompanyDTO MapCompany(Company company)
		{
			return company.ToDTO();
		}

		#endregion
	}
}
