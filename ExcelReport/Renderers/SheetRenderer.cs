using ExcelReport.Contexts;
using ExcelReport.Extends;
using ExcelReport.Meta;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelReport.Renderers
{
	public class SheetRenderer : Named
	{
		public static IList<IElementRenderer>ExtractModelToRender<T>(object model)=> new List<IElementRenderer>(ExtractModel(model).Select(k=>new ParameterRenderer(k.Key,k.Value)));


		/// <summary>
		/// 解析单个模型
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static IList<KeyValuePair<string,string>> ExtractModel(object model)
		{
			var list = new List<KeyValuePair<string, string>>();
			ExtractModel(model, ref list);
			return list;
		}
		private static void ExtractModel(object model, ref List<KeyValuePair<string, string>> elements)
		{
			var jsonStr = JsonConvert.SerializeObject(model);
			JObject jObject = (JObject)JsonConvert.DeserializeObject(jsonStr);
			ExtractModelByJsonObject(jObject, ref elements);
		}
		private static void ExtractModelByJsonObject(JToken model, ref List<KeyValuePair<string, string>> elements)
		{
			if (model == null) return;
			if(model.HasValues)ExtractModelByJsonObject(model.First,ref elements);
			else elements.Add(new KeyValuePair<string, string> (model.Path.Replace('.','_'), model.Value<object>().ToString()));
			ExtractModelByJsonObject(model.Next,ref elements);
		}
		private IList<IElementRenderer> RendererList { set; get; }

		public SheetRenderer(string sheetName, params IElementRenderer[] elementRenderers)
		{
			Name = sheetName;
			RendererList = new List<IElementRenderer>(elementRenderers);
		}

		public virtual void Render(WorkbookContext workbookContext)
		{
			var worksheetContext = workbookContext[Name];
			if (worksheetContext.IsNull() || worksheetContext.IsEmpty())
			{
				return;
			}

			if (RendererList.Count == 0) return;
			foreach (var renderer in RendererList)
			{
				renderer.Render(worksheetContext);
			}
		}
	}
}