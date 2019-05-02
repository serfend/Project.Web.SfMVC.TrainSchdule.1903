﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainSchdule.DAL.Data;
using TrainSchdule.DAL.Entities.UserInfo;
using TrainSchdule.DAL.Interfaces;

namespace TrainSchdule.DAL.Repositories
{
	public class StudentRepository:IRepository<Student>
	{
		private readonly ApplicationDbContext _context;

		public StudentRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public IEnumerable<Student> GetAll()
		{
			return _context.Students.OrderByDescending(p=>p.Birth);
		}

		public IEnumerable<Student> GetAll(int page, int pageSize)
		{
			return _context.Students.Skip(pageSize * page).Take(pageSize);
		}

		public Student Get(Guid id)
		{
			return _context.Students.FirstOrDefault(p => p.Id == id);
		}

		public Task<Student> GetAsync(Guid id)
		{
			return _context.Students.FirstOrDefaultAsync(p => p.Id == id);
		}

		public IQueryable<Student> Find(Expression<Func<Student, bool>> predicate)
		{
			return _context.Students.Where(predicate);
		}

		public void Create(Student item)
		{
			_context.Students.Add(item);
		}

		public async Task CreateAsync(Student item)
		{
			await _context.Students.AddAsync(item);
		}

		public void Update(Student item)
		{
			_context.Entry(item).State = EntityState.Modified;
		}

		public void Delete(Guid id)
		{
			var item = _context.Students.Find(id);
			if(item!=null)_context.Students.Remove(item);
		}

		public async Task DeleteAsync(Guid id)
		{
			var item = await _context.Students.FindAsync(id);
			if (item != null)  _context.Students.Remove(item);
		}
	}
}
