using BLL.Interfaces;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using DAL.Data;

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

		public Company Get(string code)
		{
			var company = _context.Companies.Find(code);
			return company;
		}

		public IEnumerable<Company> FindAllChild(string code)
		{
			return _context.Companies.Where(x => x.Parent != null && x.Parent.Code == code).ToList();
		}

		public Company FindParent(string code)
		{
			var parent = code.Length > 1 ? _context.Companies.Find(code.Substring(0, code.Length - 1)) : null;
			return parent;
		}



		public Company Create(string name,string code)
		{
			var company = CreateCompany(name,code);
			_context.Companies.Add(company);
			return company;
		}

		private Company CreateCompany(string name, string code)
		{
			var parent = FindParent(code);
			var company = new Company()
			{
				Name = name,
				Code = code,
				Parent = parent
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
			var target =await _context.Companies.FindAsync(code);
			if (target == null) return false;
			await Task.Run(() => editCallBack.Invoke(target));
			_context.Companies.Update(target);
			return true;
		}


	}
}
