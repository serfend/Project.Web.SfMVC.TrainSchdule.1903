using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;

namespace DAL.Entities.UserInfo
{
	public class Avatar:BaseEntity
	{
		public string FilePath { get; set; }
		public DateTime CreateTime { get; set; }
		[NotMapped]
		public byte[] Img { get; set; }
	}

}
