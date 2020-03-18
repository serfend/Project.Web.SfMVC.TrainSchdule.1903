using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.FileEngine
{
	public class UserFile : BaseEntity
	{
		public byte[] Data { get; set; }
	}
}