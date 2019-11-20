using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions
{
	public static class VocationExtensions
	{
		public static string ToDescription(this IEnumerable<VocationDescription> model)
		{
			var cs = new StringBuilder();
			foreach(var i in model)
			{
				cs.Append(i.Name).Append(i.Length).AppendLine("天");
			}
			return cs.ToString();
		}
	}
}
