using BLL.Extensions;
using BLL.Interfaces;
using BLL.Interfaces.Permission;
using Castle.Core.Internal;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Permisstions;
using DAL.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class CompaniesService : ICompaniesService
	{
		private readonly ApplicationDbContext _context;
        private readonly IPermissionServices permissionServices;

        public CompaniesService(ApplicationDbContext context,IPermissionServices permissionServices)
		{
			_context = context;
            this.permissionServices = permissionServices;
        }
		public List<Company> PermissionViewCompanies(User currentUser)
		{
			if (currentUser == null) return new List<Company>();
			var companyDb = _context.CompaniesDb;
			var list = _context.PermissionsUsers.Where(i=>i.UserId == currentUser.Id).Where(p=>p.Name==ApplicationPermissions.Company.Tree.Item.Key).Where(i=>i.Type.HasFlag(PermissionType.Read)).Select(i=>i.Region).DistinctByCompany().ToList();
			return list.Select(i => companyDb.FirstOrDefault(c => c.Code == i)).ToList();
		}
        public Duties GetDuties(int code)
		{
			return _context.Duties.Find(code);
		}

		public IEnumerable<Company> GetAll(int page, int pageSize)
		{
			var list = _context.CompaniesDb.Skip(page * pageSize).Take(pageSize).ToList();
			return list;
		}

		public IEnumerable<Company> GetAll(Expression<Func<Company, bool>> predicate, int page, int pageSize)
		{
			var list = _context.CompaniesDb.Where(predicate).Skip(page * pageSize).Take(pageSize);

			return list;
		}

		public Company GetById(string code) => _context.CompaniesDb.FirstOrDefault(c => c.Code == code);

		public IEnumerable<Company> FindAllChild(string code)
		{
			var list = _context.CompaniesDb.AsQueryable();
			if (code.IsNullOrEmpty() || code.ToLower() == "root") return new List<Company>();
			else
			{
				var childCodeLength = code.Length + 1;
				list = list.Where(x => x.Code.Length == childCodeLength).Where(x => x.Code.StartsWith(code));
			}
			return list.OrderBy(x => x.PrioritySum).ToList();
		}

		public Company FindParent(string code)
		{
			if (code == null) return null;
			var list = _context.CompaniesDb.AsQueryable();
			var parentCodeLength = code.Length - 1;
			var parentCode = code.Substring(0, parentCodeLength);
			list = list.Where(x => x.Code == parentCode);
			return list.FirstOrDefault();
		}

		public Company Create(string name, string code)
		{
			var company = CreateCompany(name, code);
			_context.Companies.Add(company);
			return company;
		}

		private static Company CreateCompany(string name, string code)
		{
			var company = new Company()
			{
				Name = name,
				Code = code
			};
			return company;
		}

		public async Task<Company> CreateAsync(string name, string code)
		{
			var company = CreateCompany(name, code);
			await _context.Companies.AddAsync(company).ConfigureAwait(false);
			return company;
		}

		public bool Edit(string code, Action<Company> editCallBack)
		{
			if (editCallBack == null) return true;
			var target = _context.CompaniesDb.FirstOrDefault(c => c.Code == code);
			if (target == null) return false;
			editCallBack.Invoke(target);
			_context.Companies.Update(target);
			return true;
		}

		public async Task<bool> EditAsync(string code, Action<Company> editCallBack)
		{
			var target = await _context.CompaniesDb.FirstOrDefaultAsync(c => c.Code == code).ConfigureAwait(true);
			if (target == null) return false;
			await Task.Run(() => editCallBack.Invoke(target)).ConfigureAwait(true);
			_context.Companies.Update(target);
			return true;
		}

		public IEnumerable<User> GetCompanyManagers(string code, string userid)
		{
			var r = _context.CompanyManagers.AsQueryable();
			if (code != null) r = r.Where(m => m.CompanyCode == code);
			if (userid != null) r = r.Where(m => m.UserId == userid);
			return r.Select(m => m.User);
		}

		public bool CheckManagers(string code, string userid) => _context.CompanyManagers.Where(m => m.CompanyCode == code).Any(m => m.UserId == userid);
	}
}