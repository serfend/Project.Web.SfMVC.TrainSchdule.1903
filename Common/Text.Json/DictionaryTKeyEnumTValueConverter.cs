using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Text.Json
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Text.Json;
	using System.Text.Json.Serialization;

	namespace SystemTextJsonSamples
	{
		public class DictionaryTKeyEnumTValueConverter : JsonConverterFactory
		{
			public override bool CanConvert(Type typeToConvert)
			{
				if (!typeToConvert.IsGenericType)
				{
					return false;
				}

				if (typeToConvert.GetGenericTypeDefinition() != typeof(Dictionary<,>))
				{
					return false;
				}
				var first = typeToConvert.GetGenericArguments()[0];
				if (first == typeof(int)) return true;
				return false;
			}

			public override JsonConverter CreateConverter(
				Type type,
				JsonSerializerOptions options)
			{
				Type keyType = type.GetGenericArguments()[0];
				Type valueType = type.GetGenericArguments()[1];

				JsonConverter converter = (JsonConverter)Activator.CreateInstance(
					typeof(DictionaryEnumConverterInner<,>).MakeGenericType(
						new Type[] { keyType, valueType }),
					BindingFlags.Instance | BindingFlags.Public,
					binder: null,
					args: new object[] { options },
					culture: null);

				return converter;
			}

			private class DictionaryEnumConverterInner<TKey, TValue> :
				JsonConverter<Dictionary<string, TValue>> where TKey : struct, Enum
			{
				private readonly JsonConverter<TValue> _valueConverter;
				private Type _keyType;
				private Type _valueType;

				public DictionaryEnumConverterInner(JsonSerializerOptions options)
				{
					// For performance, use the existing converter if available.
					_valueConverter = (JsonConverter<TValue>)options
						.GetConverter(typeof(TValue));

					// Cache the key and value types.
					_keyType = typeof(TKey);
					_valueType = typeof(TValue);
				}

				public override Dictionary<string, TValue> Read(
					ref Utf8JsonReader reader,
					Type typeToConvert,
					JsonSerializerOptions options)
				{
					if (reader.TokenType != JsonTokenType.StartObject)
					{
						throw new JsonException();
					}

					Dictionary<string, TValue> dictionary = new Dictionary<string, TValue>();

					while (reader.Read())
					{
						if (reader.TokenType == JsonTokenType.EndObject)
						{
							return dictionary;
						}

						// Get the key.
						if (reader.TokenType != JsonTokenType.PropertyName)
						{
							throw new JsonException();
						}

						string propertyName = reader.GetString();

						// For performance, parse with ignoreCase:false first.
						if (!int.TryParse(propertyName, out int key))
						{
							throw new JsonException(
								$"Unable to convert \"{propertyName}\" to Enum \"{_keyType}\".");
						}

						// Get the value.
						TValue v;
						if (_valueConverter != null)
						{
							reader.Read();
							v = _valueConverter.Read(ref reader, _valueType, options);
						}
						else
						{
							v = JsonSerializer.Deserialize<TValue>(ref reader, options);
						}

						// Add to dictionary.
						dictionary.Add(key.ToString(), v);
					}

					throw new JsonException();
				}

				public override void Write(
					Utf8JsonWriter writer,
					Dictionary<string, TValue> dictionary,
					JsonSerializerOptions options)
				{
					writer.WriteStartObject();

					foreach (KeyValuePair<string, TValue> kvp in dictionary)
					{
						writer.WritePropertyName(kvp.Key.ToString());

						if (_valueConverter != null)
						{
							_valueConverter.Write(writer, kvp.Value, options);
						}
						else
						{
							JsonSerializer.Serialize(writer, kvp.Value, options);
						}
					}

					writer.WriteEndObject();
				}
			}
		}
	}
}