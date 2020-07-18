using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrainSchdule.System
{
	/// <summary>
	/// Json格式化日期
	/// </summary>
	public class DateTimeConverter : JsonConverter<DateTime>
	{
		/// <summary>
		///
		/// </summary>
		public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";

		/// <summary>
		///
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="typeToConvert"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => DateTime.Parse(reader.GetString());

		/// <summary>
		///
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="value"></param>
		/// <param name="options"></param>
		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString(this.DateTimeFormat));
	}
}