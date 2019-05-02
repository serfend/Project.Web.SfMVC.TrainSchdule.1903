﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.DAL.Data;
using TrainSchdule.DAL.Entities.UserInfo;

namespace TrainSchdule.DAL.Repositories
{
    /// <summary>
    /// Contains methods for processing DB entities in UsersIdentity table.
    /// Implementation of <see cref="IRepository"/>.
    /// </summary>
    public class UsersIdentityRepository : IRepository<ApplicationUser>
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersIdentityRepository"/>.
        /// </summary>
        public UsersIdentityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Logic

        /// <summary>
        /// Method for fetching all data from <see cref="UsersIdentityRepository"/>.
        /// </summary>
        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.Users;
        }

        /// <summary>
        /// Method for fetching all data from <see cref="UsersIdentityRepository"/> with paggination.
        /// </summary>
        public IEnumerable<ApplicationUser> GetAll(int page, int pageSize)
        {
            return _context.Users.Skip(page * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Method for fetching <see cref="ApplicationUser"/> by id (primary key).
        /// </summary>
        public ApplicationUser Get(Guid id)
        {
            return _context.Users.Find(id);
        }

        /// <summary>
        /// Async method for fetching <see cref="ApplicationUser"/> by id (primary key).
        /// </summary>
        public async Task<ApplicationUser> GetAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <summary>
        /// Method for fetching <see cref="ApplicationUser"/>(s) by predicate.
        /// </summary>
        public IQueryable<ApplicationUser> Find(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return _context.Users.Where(predicate);
        }

        /// <summary>
        /// Method for creating <see cref="ApplicationUser"/>.
        /// </summary>
        public void Create(ApplicationUser item)
        {
            _context.Users.Add(item);
        }

        /// <summary>
        /// Async method for creating <see cref="ApplicationUser"/>.
        /// </summary>
        public async Task CreateAsync(ApplicationUser item)
        {
            await _context.Users.AddAsync(item);
        }

        /// <summary>
        /// Method for updating <see cref="ApplicationUser"/>.
        /// </summary>
        public void Update(ApplicationUser item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Method for deleting <see cref="ApplicationUser"/>.
        /// </summary>
        public void Delete(Guid id)
        {
            var item = _context.Users.Find(id);

            if (item != null)
            {
                _context.Users.Remove(item);
            }
        }

        /// <summary>
        /// Async method for deleting <see cref="ApplicationUser"/>.
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var item = await _context.Users.FindAsync(id);

            if (item != null)
            {
                _context.Users.Remove(item);
            }
        }

        #endregion
    }
}