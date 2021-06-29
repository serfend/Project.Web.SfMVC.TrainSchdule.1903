using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BLL.Extensions;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.Company;
using DAL.Entities;
using DAL.Entities.UserInfo;

namespace BLL.Services
{
    public class CompanyManagerServices : ICompanyManagerServices
    {
        private readonly ApplicationDbContext _context;

        public CompanyManagerServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public CompanyManagers GetById(Guid id)
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
            if (model == null) return null;
            var manager = new CompanyManagers()
            {
                AuthBy = _context.AppUsersDb.FirstOrDefault(u => u.Id == model.AuditById),
                Company = _context.CompaniesDb.FirstOrDefault(c => c.Code == model.CompanyCode),
                User = _context.AppUsersDb.FirstOrDefault(u => u.Id == model.UserId)
            };
            if (manager.Company == null || manager.User == null) return null;
            return Create(manager);
        }

        public CompanyManagers Create(CompanyManagers model)
        {
            if (model == null) return null;
            model.Create = DateTime.Now;
            _context.CompanyManagers.Add(model);
            _context.SaveChanges();
            return model;
        }

        public int Delete(CompanyManagers model)
        {
            _context.CompanyManagers.Remove(model);

            return _context.SaveChanges();
        }

        public IQueryable<User> GetMembers(string code, bool asManage) => asManage ? _context.AppUsersDb
            .Where(u => u.CompanyInfo.CompanyOfManageCode == code).OrderByCompanyAndTitle()
            : _context.AppUsersDb.Where(u => u.CompanyInfo.CompanyCode == code).OrderByCompanyAndTitle();

        public IQueryable<CompanyManagers> GetManagers(string companyCode)
            => _context.CompanyManagers.Where(c => c.CompanyCode == companyCode);
    }
}