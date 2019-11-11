using ExcelReport.Driver;
using ExcelReport.Exceptions;
using System.Collections.Generic;

namespace ExcelReport
{
	public class Configurator
	{
		private static readonly IDictionary<string, IWorkbookLoader> CONFIG = new Dictionary<string, IWorkbookLoader>();
		public IWorkbookLoader this[string suffix]
		{
			get => CONFIG[suffix];
			set
			{
				if (CONFIG.ContainsKey(suffix))
					CONFIG[suffix] = value;
				else CONFIG.Add(suffix, value);
			}
		}

	}
}