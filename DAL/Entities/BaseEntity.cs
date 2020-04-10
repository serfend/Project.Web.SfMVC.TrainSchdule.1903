using System;

namespace DAL.Entities
{
	/// <summary>
	/// Base class for entities.
	/// Contains <see cref="Id"/> property.
	/// </summary>
	public class BaseEntity
	{
		/// <summary>
		/// Gets and sets id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// 使用软删除
		/// </summary>
		public bool IsRemoved { get; set; }

		/// <summary>
		/// 删除时间
		/// </summary>
		public DateTime IsRemovedDate { get; set; }

		public void Remove()
		{
			this.IsRemoved = true;
			this.IsRemovedDate = DateTime.Now;
		}
	}
}