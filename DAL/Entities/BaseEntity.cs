using System;
using System.ComponentModel.DataAnnotations;

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
    }
}
