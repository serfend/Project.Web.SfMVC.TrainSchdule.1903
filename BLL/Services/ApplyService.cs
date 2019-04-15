using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Extensions;
using BLL.Interfaces;
using DAL.Entities;
using TrainSchdule.DAL.Interfaces;

namespace BLL.Services
{
	public class ApplyService:IApplyService
	{
		#region Fileds

		private readonly IUnitOfWork _unitOfWork;

		#endregion
		public ApplyService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		#region Dispose

		private bool _isDisposed;

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

		~ApplyService()
		{
			Dispose(false);
		}
		#endregion

		public IEnumerable<ApplyDTO> GetAll(int page, int pageSize)
		{
			var list = new List<ApplyDTO>();
			_unitOfWork.Applies.GetAll(page, pageSize).All(item =>
			{
				list.Add(item.ToDTO());
				return true;
			});
			return list;
		}

		public IEnumerable<ApplyDTO> GetAll(string userName)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<ApplyDTO> GetAll(string userName, AuditStatus status)
		{
			throw new NotImplementedException();
		}

		public Apply Create(Apply item)
		{
			throw new NotImplementedException();
		}

		public Task<Apply> CreateAsync(Apply item)
		{
			throw new NotImplementedException();
		}

		public bool Edit(string userName, Action<Apply> editCallBack)
		{
			throw new NotImplementedException();
		}

		public Task<bool> EditAsync(string userName, Action<Apply> editCallBack)
		{
			throw new NotImplementedException();
		}
	}
}
