using ExcelReport.Common;
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
		/// <summary>
		/// 将对象进行匹配
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="model"></param>
		/// <param name="Filter">匹配当key，value决定时，如何修改value</param>
		/// <returns></returns>
		public static IList<IElementRenderer> ExtractModelToRender<T>(object model, Func<string, string, string> Filter = null) => new List<IElementRenderer>(ExtractModel(model).Select(k => new ParameterRenderer(k.Key, Filter != null ? Filter.Invoke(k.Key, k.Value) : k.Value)));

		public static IList<KeyValuePair<string, Func<T, string>>> ExtractIEmbeddedModel<T>(object model) where T : class
		{
			var info = model.GetType();
			info.GetProperties()[0].GetValue(model);
			return new List<KeyValuePair<string, Func<T, string>>>();
		}

		/// <summary>
		/// 解析单个模型
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static IList<KeyValuePair<string, string>> ExtractModel(object model)
		{
			var list = new List<KeyValuePair<string, string>>();
			ExtractModel(model, ref list);
			return list;
		}

		private static void ExtractModel(object model, ref List<KeyValuePair<string, string>> elements, string path = "")
		{
			if (model == null)
			{
				elements.Add(new KeyValuePair<string, string>(path, ""));
				return;
			}
			var type = model.GetType();
			if (type.IsValueType || type.Name == "String")
			{
				elements.Add(new KeyValuePair<string, string>(path, model.CastToString(type)));
			}
			else
			{
				if (type.Name == "List`1") return;
				var children = type.GetProperties();
				foreach (var child in children)
				{
					var indent = path.Length == 0 ? string.Empty : "_";
					if (child.Name == "LazyLoader" || child.Name == "Comparer" || child.Name == "Instance" || child.Name == "SyncRoot") continue;
					ExtractModel(child.GetValue(model), ref elements, $"{path}{indent}{child.Name}");
				}
			}

			//var jsonStr = JsonConvert.SerializeObject(model);
			//JObject jObject = (JObject)JsonConvert.DeserializeObject(jsonStr);
			//ExtractModelByJsonObject(jObject, ref elements);
		}

		private static void ExtractModelByJsonObject(JToken model, ref List<KeyValuePair<string, string>> elements)
		{
			if (model == null) return;

			if (model.HasValues) ExtractModelByJsonObject(model.First, ref elements);
			else
			{
				var rawValue = model.Value<object>();
				string value = rawValue.ToString();
				elements.Add(new KeyValuePair<string, string>(model.Path.Replace('.', '_'), value));
			}
			ExtractModelByJsonObject(model.Next, ref elements);
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