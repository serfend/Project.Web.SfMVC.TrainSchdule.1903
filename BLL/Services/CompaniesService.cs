using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TrainSchdule.BLL.DTO.UserInfo;
using TrainSchdule.BLL.Extensions;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.DAL.Entities.UserInfo;
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
		public Company GetCompanyByPath(string code)
		{
			var company =  _unitOfWork.Companies.Find(x => x.Code == code).FirstOrDefault();
			return company;
		}
		public IEnumerable<CompanyDTO> GetAll(int page, int pageSize)
		{
			var list = _unitOfWork.Companies.GetAll(page, pageSize).ToList();
			var result = new List<CompanyDTO>(list.Count());
			foreach (var company in list)
			{
				result.Add(MapCompany(company));
			}
			return result;
		}

		public IEnumerable<CompanyDTO> GetAll(Expression<Func<Company, bool>> predicate, int page, int pageSize)
		{
			var list= _unitOfWork.Companies.Find(predicate).Skip(page*pageSize).Take(pageSize);
			var result=new List<CompanyDTO>(list.Count());
			foreach (var company in list)
			{
				result.Add(MapCompany(company));
			}
			return result;
		}

		public CompanyDTO Get(string code)
		{
			var company = GetCompanyByPath(code);
			return MapCompany(company);
		}

		public IEnumerable<CompanyDTO> FindAllChild(string code)
		{
			return _unitOfWork.Companies.Find(x => x.Parent != null && x.Parent.Code==code).ToList().Select(MapCompany);
		}




		public Company Create(string name,string code)
		{
			var parent =code.Length>1? GetCompanyByPath(code.Substring(0, code.Length - 1)):null;
			var company=new Company()
			{
				Name = name,
				Code = code,
				Parent = parent
			};
			_unitOfWork.Companies.Create(company);
			_unitOfWork.Save();
			return company;
		}

		public async Task<Company> CreateAsync(string name,string code)
		{
			var parent = code.Length > 1 ? GetCompanyByPath(code.Substring(0, code.Length - 1)) : null;
			var company=new Company()
			{
				Name = name,
				Code = code,
				Parent = parent
			};
			await  _unitOfWork.Companies.CreateAsync(company);
			await _unitOfWork.SaveAsync();
			return company;
		}

		public bool Edit(string path, Action<Company> editCallBack)
		{
			var target = _unitOfWork.Companies.Find((item) => item.Code == path).FirstOrDefault();
			if (target == null) return false;
			editCallBack.Invoke(target);
			_unitOfWork.Companies.Update(target);
			_unitOfWork.Save();
			return true;
		}

		public async Task<bool> EditAsync(string path, Action<Company> editCallBack)
		{
			var target = _unitOfWork.Companies.Find((item) => item.Code == path).FirstOrDefault();
			if (target == null) return false;
			editCallBack.Invoke(target);
			_unitOfWork.Companies.Update(target);
			await _unitOfWork.SaveAsync();
			return true;
		}


		#region Helpers

		/// <summary>
		/// 
		/// </summary>
		protected CompanyDTO MapCompany(Company company)
		{
			return company.ToDTO();
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
		#endregion
	}
}
