using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace ExcelReport.Common
{
	public static class EnumValueCast
	{
		public static string CastToString(this object v, Type t)
		{
			if (!t.IsEnum)
				return v.ToString();
			try
			{
				FieldInfo oFieldInfo = t.GetField(GetName(t, v));
				DescriptionAttribute[] attributes = (DescriptionAttribute[])oFieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
				return (attributes.Length > 0) ? attributes[0].Description : GetName(t, v);
			}
			catch
			{
				return "UNKNOWN";
			}
		}
		private static string GetName(System.Type t, object v)
		{
			try
			{
				return Enum.GetName(t, v);
			}
			catch
			{
				return "UNKNOWN";
			}
		}
	}

}
