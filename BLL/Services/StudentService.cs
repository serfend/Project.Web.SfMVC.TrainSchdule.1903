using System;
using System.Collections.Generic;
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
	public	class StudentService:IStudentService
	{

		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ICurrentUserService _currentUserService;

		public StudentService(ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
		{
			_currentUserService = new CurrentUserService(unitOfWork,httpContextAccessor);
			_httpContextAccessor = httpContextAccessor;
			_unitOfWork = unitOfWork;
		}



		public List<FilterDTO> Filters { get; set; }
		public List<TagDTO> Tags { get; set; }
		public IEnumerable<StudentDTO> GetAll(int page, int pageSize)
		{
			var students= _unitOfWork.Students.GetAll(page, pageSize);
			var list=new List<StudentDTO>(pageSize);
			foreach (var student in students)
			{
				list.Add(student.ToDTO());
			}

			return list;
		}

		public StudentDTO Get(Guid id)
		{
			var student = _unitOfWork.Students.Get(id);
			return student.ToDTO();
		}

		public async Task<StudentDTO> GetAsync(Guid id)
		{
			var student = await _unitOfWork.Students.GetAsync(id);
			return student.ToDTO();
		}

		public Guid Create(StudentDTO item)
		{
			var student = item.ToEntity();
			_unitOfWork.Students.Create(student);
			_unitOfWork.Save();
			item.id = student.Id;
			return student.Id;
		}

		public async ValueTask<Guid> CreateAsync(StudentDTO item)
		{
			var student = item.ToEntity();
			await _unitOfWork.Students.CreateAsync(student);
			await _unitOfWork.SaveAsync();
			item.id = student.Id;
			return student.Id;
		}

		public void Edit(Guid id, StudentDTO item)
		{
			//var student = _unitOfWork.Students.Get(id);
			//if (student == null) return;
			var s = item.ToEntity();
			s.Id = id;
			_unitOfWork.Students.Update(s);
			_unitOfWork.Save();
		}

		public async Task EditAsync(Guid id, StudentDTO item)
		{
			var s = item.ToEntity();
			s.Id = id;
			_unitOfWork.Students.Update(s);
			await _unitOfWork.SaveAsync();
		}

		public void Delete(Guid id)
		{
			_unitOfWork.Students.Delete(id);
			_unitOfWork.Save();
		}

		public async Task DeleteAsync(Guid id)
		{
			await _unitOfWork.Students.DeleteAsync(id);
			await _unitOfWork.SaveAsync();
		}


		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private bool _isDisposed = false;
		public virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing)
				{
					_unitOfWork.Dispose();
					_currentUserService.Dispose();
				}

				_isDisposed = true;
			}
		}
	}
}
