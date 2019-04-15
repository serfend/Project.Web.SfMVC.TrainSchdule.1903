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
			return GetAll((item) => true,page,pageSize);
		}

		public IEnumerable<ApplyDTO> GetAll(string userName,int page,int pageSize)
		{
			return GetAll((item) => item.From.UserName == userName,page,pageSize);
		}

		public IEnumerable<ApplyDTO> GetAll(string userName, AuditStatus status,int page,int pageSize)
		{
			return GetAll((item) => item.From.UserName == userName && status == item.Status,page,pageSize);
		}

		public IEnumerable<ApplyDTO> GetAll(Func<Apply,bool> predicate, int page, int pageSize)
		{
			var list = new List<ApplyDTO>();
			var items = _unitOfWork.Applies.Find(predicate).Skip(page * pageSize).Take(pageSize);
			items.All((item) =>
			{
				list.Add(item.ToDTO());
				return true;
			});
			return list;
		}
		public Apply Create(Apply item)
		{
			_unitOfWork.Applies.Create(item);
			_unitOfWork.Save();
			return item;
		}

		public async Task<Apply> CreateAsync(Apply item)
		{
			await _unitOfWork.Applies.CreateAsync(item);
			await _unitOfWork.SaveAsync();
			return item;
		}

		public bool Edit(string userName, Action<Apply> editCallBack)
		{
			var target = _unitOfWork.Applies.Find((item) => item.From.UserName == userName).FirstOrDefault();
			if (target == null) return false;
			editCallBack.Invoke(target);
			_unitOfWork.Applies.Update(target);
			_unitOfWork.Save();
			return true;
		}

		public async Task<bool> EditAsync(string userName, Action<Apply> editCallBack)
		{
			var target = _unitOfWork.Applies.Find((item) => item.From.UserName == userName).FirstOrDefault();
			if (target == null) return false;
			editCallBack.Invoke(target);
			_unitOfWork.Applies.Update(target);
			await _unitOfWork.SaveAsync();
			return true;
		}
	}
}
