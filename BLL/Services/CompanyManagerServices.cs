using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.Company;
using DAL.Entities;

namespace BLL.Services
{
	public class CompanyManagerServices: ICompanyManagerServices
	{
		private readonly ApplicationDbContext _context;

		public CompanyManagerServices(ApplicationDbContext context)
		{
			_context = context;
		}

		public CompanyManagers Get(Guid id)
		{
			return _context.CompanyManagers.Find(id);
		}

		public IEnumerable<CompanyManagers> GetAll(Expression<Func<CompanyManagers, bool>> predicate, int page, int pageSize)
		{
			return _context.CompanyManagers.Where(predicate).Skip(page * pageSize).Take(pageSize);
		}

		public int Edit(CompanyManagers model)
		{
			_context.CompanyManagers.Update(model);
			
			return _context.SaveChanges();
		}

		public CompanyManagers CreateManagers(CompanyManagerVdto model)
		{
			var manager=new CompanyManagers()
			{
				AuthBy = _context.AppUsers.Find(model.AuditById),
				Company = _context.Companies.Find(model.CompanyCode),
				User = _context.AppUsers.Find(model.UserId)
			};
			if (manager.Company == null || manager.User == null) return null;
			return Create(manager);
		}

		public CompanyManagers Create(CompanyManagers model)
		{
			model.Create=DateTime.Now;
			_context.CompanyManagers.Add(model);
			_context.SaveChanges();
			return model;
		}

		public int Delete(CompanyManagers model)
		{
			_context.CompanyManagers.Remove(model);
			
			return _context.SaveChanges();
		}

		public CompanyManagers GetManagerByUC(string userId, string companyCode)
		{
			return _context.CompanyManagers.Where(m => m.User.Id == userId).SingleOrDefault(m => m.Company.Code == companyCode);
		}
	}
}
