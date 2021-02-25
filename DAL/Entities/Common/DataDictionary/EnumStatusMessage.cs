using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Common.DataDictionary
{

	public class EnmuDescriptionItem
	{
		public EnmuDescriptionItem()
		{
		}

		public EnmuDescriptionItem(int code, string message, string desc, string color)
		{
			Code = code;
			Message = message;
			Desc = desc;
			Color = color;
		}

		public int Code { get; set; }
		public string Message { get; set; }
		public string Desc { get; set; }
		public string Color { get; set; }

	}
}
