using BLL.Interfaces;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class CompaniesService:ICompaniesService
	{
		private readonly ApplicationDbContext _context;

		public CompaniesService(ApplicationDbContext context)
		{
			_context = context;
		}

		public IEnumerable<Company> GetAll(int page, int pageSize)
		{
			var list = _context.Companies.Skip(page*pageSize).Take(pageSize).ToList();
			return list;
		}

		public IEnumerable<Company> GetAll(Expression<Func<Company, bool>> predicate, int page, int pageSize)
		{
			var list= _context.Companies.Where(predicate).Skip(page*pageSize).Take(pageSize);

			return list;
		}

		public Company GetById(string code)
		{
			var company = _context.Companies.Find(code);
			return company;
		}

		public IEnumerable<Company> FindAllChild(string code)
		{
			if (code.ToLower() == "root") return _context.Companies.Where(x => x.Code.Length == 1).ToList();
			return _context.Companies.Where(x => ParentCode(x.Code) == code).ToList();
		}

		public Company FindParent(string code)
		{
			var parent = ParentCode(code);
			return _context.Companies.Find(parent);
		}

		private string ParentCode(string code)=>(code != null && code.Length > 1) ? code.Substring(0, code.Length - 1) : null;

		public Company Create(string name,string code)
		{
			var company = CreateCompany(name,code);
			_context.Companies.Add(company);
			return company;
		}

		private Company CreateCompany(string name, string code)
		{
			var company = new Company()
			{
				Name = name,
				Code = code
			};
			return company;
		}
		public async Task<Company> CreateAsync(string name,string code)
		{
			var company = CreateCompany(name,code);
			await  _context.Companies.AddAsync(company);
			return company;
		}

		public bool Edit(string code, Action<Company> editCallBack)
		{
			var target = _context.Companies.Find(code);
			if (target == null) return false;
			editCallBack.Invoke(target);
			_context.Companies.Update(target);
			return true;
		}

		public async Task<bool> EditAsync(string code, Action<Company> editCallBack)
		{
			var target =await _context.Companies.FindAsync(code).ConfigureAwait(true);
			if (target == null) return false;
			await Task.Run(() => editCallBack.Invoke(target)).ConfigureAwait(true);
			_context.Companies.Update(target);
			return true;
		}

		public IEnumerable<User> GetCompanyManagers(string code)
		{
			var target = _context.Companies.Find(code);
			if (target == null) return null;
			return _context.CompanyManagers.Where(m => m.Company.Code == target.Code).Select(m=>m.User);
		}

		public bool CheckManagers(string code, string userid)
		{
			return _context.CompanyManagers.Where(m => m.Company.Code == code).Any(m => m.User.Id == userid);
		}
	}
}
