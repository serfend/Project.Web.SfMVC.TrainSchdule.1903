using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
	public class CompaniesService:ICompaniesService
	{
		
		private readonly IUnitOfWork _unitOfWork;
		
		#region Disposing
		private bool _isDisposed;

		public CompaniesService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
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

		~CompaniesService()
		{
			Dispose(false);
		}

		#endregion
		public Company GetCompanyByPath(string path)
		{
			var company =  _unitOfWork.Companies.Find(x => x.Path == path).FirstOrDefault();
			return company;
		}
		public IEnumerable<CompanyDTO> GetAll(int page, int pageSize)
		{
			var Companies= _unitOfWork.Companies.GetAll(page, pageSize);
			var result = new List<CompanyDTO>(Companies.Count());
			foreach (var company in Companies)
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

		public IEnumerable<CompanyDTO> FindAllChild(Guid id)
		{
			var list = _unitOfWork.Companies.Find(x => x.Parent != null && x.Parent.Id==id);
			var result=new List<CompanyDTO>(list.Count());
			foreach (var company in list)
			{
				result.Add(MapCompany(company));
			}

			return result;
		}

		public void SetParent(Guid id, string parentPath)
		{
			var company = _unitOfWork.Companies.Get(id);
			var parentCompany = GetCompanyByPath(parentPath);
			parentCompany = CheckParentCompany(parentCompany, parentPath);
			if (company != null)
			{
				company.Parent = parentCompany;
				company.Path = $"{parentCompany.Path}/{company.Name}";
				_unitOfWork.Save();
			}
		}

		private Company CheckParentCompany( Company parentCompany,string parentName)
		{
			if (parentCompany == null)
			{
				if (parentName == "Root")
				{
					return Create(parentName);
				}else 
				throw  new CompanyNotExistException("父单位尚未被创建");
			}

			return parentCompany;
		}
		public async Task SetParentAsync(Guid id, string parentPath)
		{
			var company = _unitOfWork.Companies.Get(id);
			var parentCompany = GetCompanyByPath(parentPath);
			parentCompany=CheckParentCompany( parentCompany, parentPath);
			if (company != null)
			{
				company.Parent = parentCompany;
				company.Path = $"{parentCompany.Path}/{company.Name}";
				await _unitOfWork.SaveAsync();
			}
		}
		[Serializable]
		public class CompanyNotExistException : Exception
		{
			//
			// For guidelines regarding the creation of new exception types, see
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
			// and
			//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
			//

			public CompanyNotExistException()
			{
			}

			public CompanyNotExistException(string message) : base(message)
			{
			}

			public CompanyNotExistException(string message, Exception inner) : base(message, inner)
			{
			}

			protected CompanyNotExistException(
				SerializationInfo info,
				StreamingContext context) : base(info, context)
			{
			}
		}

		public Company Create(string name)
		{
			var company=new Company()
			{
				Name = name,
				Path = name
			};
			_unitOfWork.Companies.Create(company);
			return company;
		}

		public async Task<Company> CreateAsync(string name)
		{
			var company=new Company()
			{
				Name = name,
				Path = name
			};
			await  _unitOfWork.Companies.CreateAsync(company);
			return company;
		}


		#region Helpers

		/// <summary>
		/// 
		/// </summary>
		protected CompanyDTO MapCompany(Company company)
		{
			return company.ToDTO();
		}

		#endregion
	}
}
