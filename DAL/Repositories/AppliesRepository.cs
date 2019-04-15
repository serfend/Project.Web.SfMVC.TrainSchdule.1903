using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using TrainSchdule.DAL.Data;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.DAL.Repositories;

namespace DAL.Repositories
{
		/// <summary>
		/// Contains methods for processing DB entities in Companies table.
		/// Implementation of <see cref="IRepository{T}"/>.
		/// </summary>
		public class AppliesRepository : IRepository<Apply>
		{
			#region Fields

			private readonly ApplicationDbContext _context;

			#endregion

			#region .ctors

			/// <summary>
			/// Initializes a new instance of the <see cref="AppliesRepository"/>.
			/// </summary>
			public AppliesRepository(ApplicationDbContext context)
			{
				_context = context;
			}

			#endregion

			#region Logic

			/// <summary>
			/// Method for fetching all data from <see cref="AppliesRepository"/>.
			/// </summary>
			public IEnumerable<Apply> GetAll()
			{
				return _context.Applies;
			}

			/// <summary>
			/// Method for fetching all data from <see cref="AppliesRepository"/> with paggination.
			/// </summary>
			public IEnumerable<Apply> GetAll(int page, int pageSize)
			{
				return _context.Applies.Skip(page * pageSize).Take(pageSize);
			}

			/// <summary>
			/// Method for fetching <see cref="Apply"/>.by id (primary key).
			/// </summary>
			public Apply Get(Guid id)
			{
				return _context.Applies.Find(id);
			}

			/// <summary>
			/// Async method for fetching <see cref="Apply"/>.by id (primary key).
			/// </summary>
			public async Task<Apply> GetAsync(Guid id)
			{
				return await _context.Applies.FindAsync(id);
			}

			/// <summary>
			/// Method for fetching <see cref="Apply"/>.s) by predicate.
			/// </summary>
			public IEnumerable<Apply> Find(Func<Apply, bool> predicate)
			{
				return _context.Applies.Where(predicate);
			}

			/// <summary>
			/// Method for creating <see cref="Apply"/>.
			/// </summary>
			public void Create(Apply item)
			{
				_context.Applies.Add(item);
			}

			/// <summary>
			/// Async method for creating <see cref="Apply"/>.
			/// </summary>
			public async Task CreateAsync(Apply item)
			{
				await _context.Applies.AddAsync(item);
			}

			/// <summary>
			/// Method for updating <see cref="Apply"/>.
			/// </summary>
			public void Update(Apply item)
			{
				_context.Entry(item).State = EntityState.Modified;
			}

			/// <summary>
			/// Method for deleting <see cref="Apply"/>.
			/// </summary>
			public void Delete(Guid id)
			{
				var item = _context.Applies.Find(id);

				if (item != null)
				{
					_context.Applies.Remove(item);
				}
			}

			/// <summary>
			/// Async method for deleting <see cref="Apply"/>.
			/// </summary>
			public async Task DeleteAsync(Guid id)
			{
				var item = await _context.Applies.FindAsync(id);

				if (item != null)
				{
					_context.Applies.Remove(item);
				}
			}

			#endregion
		}
}
