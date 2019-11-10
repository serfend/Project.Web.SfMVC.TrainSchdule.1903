using ExcelReport.Driver;
using ExcelReport.Exceptions;
using System.Collections.Generic;

namespace ExcelReport
{
    public static class Configurator
    {
        private static readonly IDictionary<string, IWorkbookLoader> CONFIG = new Dictionary<string, IWorkbookLoader>();

        public static void Put(string suffix, IWorkbookLoader workbookLoader)
        {
			var config = CONFIG.TryGetValue(suffix, out var value);
            if (config)
            {
                CONFIG[suffix] = workbookLoader;
            }
            else
            {
                CONFIG.Add(suffix, workbookLoader);
            }
        }

        public static IWorkbookLoader Get(string suffix)
        {
            if (CONFIG.ContainsKey(suffix))
            {
                return CONFIG[suffix];
            }
            throw new UnconfiguredException(suffix);
        }
    }
}