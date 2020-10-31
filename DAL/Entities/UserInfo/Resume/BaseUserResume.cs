using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.UserInfo.Resume
{
	public class BaseUserResume<T> : BaseEntityGuid where T : class
	{
		public virtual T Model { get; set; }
		public DateTime Start { get; set; }
	}
}