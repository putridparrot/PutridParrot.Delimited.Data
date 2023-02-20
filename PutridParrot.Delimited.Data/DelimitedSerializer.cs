using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using PutridParrot.Delimited.Data.Attributes;
using PutridParrot.Delimited.Data.Utils;
using System.ComponentModel;
using PutridParrot.Delimited.Data.Exceptions;

namespace PutridParrot.Delimited.Data
{
	/// <summary>
	/// Allows us to serialize/deserialize to/from objects
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class DelimitedSerializer<T> where T : new()
	{
		private const string Property = "Property";
		private const string Heading = "Heading";
		private const string ColumnIndex = "ColumnIndex";
		private const string Mapping = "Mapping";
		private const string SerializerMappings = "DelimitedSerializerMappings";
		private const string Required = "Required";

		private static List<TAssocType> GetAssociations<TAssocType, TAttributeType>(Type type, Func<PropertyInfo, TAttributeType, TAssocType> factory)
		{
			var associations = new List<TAssocType>();

			var properties = type.GetProperties();
			foreach (var pi in properties)
			{
                if (pi.GetCustomAttributes(typeof(TAttributeType), true) is TAttributeType[] attributes && attributes.Length > 0)
				{
					associations.Add(factory.Invoke(pi, attributes[0]));
				}
			}

			return associations;
		}

		private static FieldReadProperty ReadAttributeFactory(PropertyInfo propertyInfo, DelimitedFieldReadAttribute attribute)
		{
			return new FieldReadProperty(propertyInfo, attribute);
		}

		private static FieldWriteProperty WriteAttributeFactory(PropertyInfo propertyInfo, DelimitedFieldWriteAttribute attribute)
		{
			return new FieldWriteProperty(propertyInfo, attribute);
		}

		public static void Serialize(IDelimitedSeparatedWriter delimiterSeparatedWriter, Stream stream, IList<T> list, DelimitedSerializeOptions options)
		{
			var attributes = options != null && options.Mappings != null ? new List<FieldWriteProperty>(options.Mappings) :
				GetAssociations<FieldWriteProperty, DelimitedFieldWriteAttribute>(typeof(T), WriteAttributeFactory);

			attributes.Sort((a, b) =>
			{
				if (a.Value.ColumnIndex == -1)
					a.Value.ColumnIndex = attributes.Count;
				if (b.Value.ColumnIndex == -1)
					b.Value.ColumnIndex = attributes.Count;

				return a.Value.ColumnIndex - b.Value.ColumnIndex;
			});

            using var writer = new DelimitedStreamWriter(delimiterSeparatedWriter, stream);

            if (options != null && options.IncludeHeadings)
            {
                writer.WriteLine(attributes.Select(association => association.Value.Heading));
            }

            if (list != null)
            {
                foreach (T t in list)
                {
                    Serialize(writer, t, attributes);
                }
            }
        }

		private static void Serialize(DelimitedStreamWriter writer, T instance, List<FieldWriteProperty> attributes)
		{
			IList<string> items = new List<string>(attributes.Count);
			foreach (var association in attributes)
			{
				var value = association.Key.GetValue(instance, null);
				items.Add(value != null ? value.ToString() : String.Empty);
			}
			writer.WriteLine(items);
		}

		private static void Clear(Dictionary<FieldReadProperty, bool> dictionary)
		{
			if (dictionary != null)
			{
				var keys = new List<FieldReadProperty>(dictionary.Keys);
				foreach (FieldReadProperty a in keys)
				{
					dictionary[a] = false;
				}
			}
		}

		private static bool AreSet(Dictionary<FieldReadProperty, bool> dictionary, bool value)
		{
			return dictionary == null || dictionary.Keys.All(a => dictionary[a] == value);
		}

		private static int FindByHeading(IEnumerable<FieldReadProperty> fields, FieldReadProperty value)
		{
			int idx = 0;
			foreach (FieldReadProperty kvp in fields)
			{
				if (kvp.Value.Heading.Equals(value.Value.Heading, StringComparison.CurrentCultureIgnoreCase))
					return idx;
				idx++;
			}
			return -1;
		}

		private static int FindByAlternateNames(IEnumerable<FieldReadProperty> fields, FieldReadProperty value)
		{
			int idx = 0;
			foreach (FieldReadProperty kvp in fields)
			{
				if (kvp.Value.AlternateNames != null)
				{
					if (kvp.Value.AlternateNames.Any(alternateName => value.Value.Heading.Equals(alternateName, StringComparison.CurrentCultureIgnoreCase)))
					{
						return idx;
					}
				}
				idx++;
			}
			return -1;
		}

		private static int GetColumnHeadingIndex(IList<FieldReadProperty> fields, FieldReadProperty value)
		{
			int idx = value.Value.Heading == null ? value.Value.ColumnIndex : FindByHeading(fields, value);
			return idx < 0 ? FindByAlternateNames(fields, value) : idx;
		}

		private static IList<TPropertyType> IterateOverMappingStream<TPropertyType>(Stream mappingStream, Func<PropertyInfo[], IDictionary<string, string>, TPropertyType> builder)
		{
			List<TPropertyType> properties = null;

			using (XmlReader reader = XmlReader.Create(mappingStream))
			{
				if (reader.IsEmptyElement)
				{
					reader.ReadStartElement();
					return null;
				}

				reader.MoveToContent();

				reader.ReadStartElement(SerializerMappings);
				if (!reader.IsEmptyElement)
				{
					properties = new List<TPropertyType>();

					PropertyInfo[] p = typeof(T).GetProperties();

					while (reader.Read())
					{
						if (reader.Name == Mapping)
						{
							if (reader.HasAttributes)
							{
								var attributes = new Dictionary<string, string>();
								while (reader.MoveToNextAttribute())
								{
									attributes.Add(reader.Name, reader.Value);
								}

								TPropertyType pt = builder(p, attributes);
								if (!EqualityComparer<TPropertyType>.Default.Equals(pt, default(TPropertyType)))
								{
									properties.Add(pt);
								}
							}
							reader.Read();
						}
					}
				}
			}

			return properties;
		}

		public static IList<FieldReadProperty> GenerateReadMappings(Stream mappingStream)
		{
			return IterateOverMappingStream(mappingStream, (properties, attributes) => 
				{
					attributes.TryGetValue(Property, out var property);
					attributes.TryGetValue(Heading, out var heading);

					var index = -1;
					if (attributes.TryGetValue(ColumnIndex, out var columnIndex))
					{
						index = Convert.ToInt32(columnIndex, CultureInfo.CurrentCulture);
					}

					var required = false;
					if(attributes.TryGetValue(Required, out var requiredString))
					{
						required = Convert.ToBoolean(requiredString, CultureInfo.CurrentCulture);
					}

					var propertyInfo = properties.FirstOrDefault(p => p.Name == property);
					return (propertyInfo != null) ?
						new FieldReadProperty(propertyInfo, new DelimitedFieldReadAttribute(heading) { ColumnIndex = index, Required = required }) : null;
				});
		}

		public static IList<FieldWriteProperty> GenerateWriteMappings(Stream mappingStream)
		{
			return IterateOverMappingStream(mappingStream, (properties, attributes) => 
				{
                    attributes.TryGetValue(Property, out var property);
                    attributes.TryGetValue(Heading, out var heading);

					var index = -1;
                    if (attributes.TryGetValue(ColumnIndex, out var columnIndex))
					{
						index = Convert.ToInt32(columnIndex, CultureInfo.CurrentCulture);
					}

					var propertyInfo = properties.FirstOrDefault(p => p.Name == property);
					return propertyInfo != null ?
						new FieldWriteProperty(propertyInfo, new DelimitedFieldWriteAttribute(heading) { ColumnIndex = index }) : 
                        null;
				});
		}

		public static IEnumerable<T> Deserialize(IDelimitedSeparatedReader delimiterSeparatedReader, string data)
		{
			return Deserialize(delimiterSeparatedReader, data, null);
		}

		public static IEnumerable<T> Deserialize(IDelimitedSeparatedReader delimiterSeparatedReader, string data, DelimitedDeserializeOptions options)
        {
            using var memoryStream = new MemoryStream();

            memoryStream.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);

            foreach (var item in Deserialize(delimiterSeparatedReader, memoryStream, options))
            {
                yield return item;
            }
        }

		public static IEnumerable<T> Deserialize(IDelimitedSeparatedReader delimiterSeparatedReader, Stream stream)
		{
			return Deserialize(delimiterSeparatedReader, stream, null);
		}

		public static IEnumerable<T> Deserialize(IDelimitedSeparatedReader delimiterSeparatedReader, Stream stream, DelimitedDeserializeOptions options)
		{
			var attributes = options != null && options.Mappings != null ? options.Mappings :
				GetAssociations<FieldReadProperty, DelimitedFieldReadAttribute>(typeof(T), ReadAttributeFactory);

			var required = new Dictionary<FieldReadProperty, bool>();
			if (options != null && options.EnforceRequiredFields)
			{
				foreach (var assoc in attributes)
				{
					if (assoc.Value.Required)
					{
						required.Add(assoc, false);
					}
				}
			}

            using var reader = new DelimitedStreamReader(delimiterSeparatedReader, stream);

            // remove any "ignore rows"
            if (options != null && options.IgnoreFirstNRows > 0)
            {
                var nRow = 0;
                while (nRow < options.IgnoreFirstNRows && reader.ReadLine() != null)
                {
                    nRow++;
                }
            }

            if (options != null && !options.UseHeadings)
            {
                // we don't want to have to create this dummy every time, so create here and set-up later
                var dummy = new FieldReadProperty(null, new DelimitedFieldReadAttribute());

                IList<string> fields;
                while ((fields = reader.ReadLine()) != null)
                {
                    var newItem = new T();
                    for (var i = 0; i < fields.Count; i++)
                    {
                        dummy.Value.ColumnIndex = i;
                        ApplyToProperty(attributes, i, fields, dummy, newItem);
                    }
                    yield return newItem;
                }
            }
            else
            {
                var headings = reader.ReadLine();
                if (headings != null)
                {
                    // we don't want to have to create this dummy every time, so create here and set-up later
                    var dummy = new FieldReadProperty(null, new DelimitedFieldReadAttribute());

                    var hasHeadings = false;
                    foreach (var heading in headings)
                    {
                        dummy.Value.Heading = heading;
                        var pos = GetColumnHeadingIndex(attributes, dummy);
                        if (pos >= 0)
                        {
                            hasHeadings = true;
                            break;
                        }
                    }

                    if (!hasHeadings)
                    {
                        throw new DelimitedSerializationException("Expected to find heading names within the first row of the data but none were found.");
                    }

                    IEnumerable<string> fields;
                    while ((fields = reader.ReadLine()) != null)
                    {
                        if (options != null && options.IgnoreEmptyRows)
                        {
                            if (fields.All(String.IsNullOrEmpty))
                                continue;
                        }

                        var fieldCount = fields.Count();
                        var newItem = new T();
                        for (var i = 0; i < headings.Count; i++)
                        {
                            if (i < fieldCount)
                            {
                                dummy.Value.Heading = headings.ElementAt(i);
                                ApplyToProperty(attributes, i, fields, dummy, newItem);
                            }
                        }
                        yield return newItem;
                        if (!AreSet(required, true))
                        {
                            var sb = new StringBuilder("One or more required fields were not supplied. Requires ");
                            var firstItem = false;
                            foreach (var a in required.Keys.Where(a => required[a] == false))
                            {
                                if (firstItem)
                                {
                                    sb.Append(", ");
                                }
                                sb.Append(a.Value.Heading);
                                firstItem = true;
                            }
                            throw new DelimitedSerializationException(sb.ToString());
                        }
                        // clear the required fields
                        Clear(required);
                    }
                }
            }
        }

		private static void ApplyToProperty(IList<FieldReadProperty> attributes, int i, IEnumerable<string> fields, FieldReadProperty dummy, T newItem)
		{
			var pos = GetColumnHeadingIndex(attributes, dummy);
			if (pos >= 0)
			{
				ChangeType(attributes[pos].Key, fields, i, newItem);
			}
		}

		private static void ChangeType(PropertyInfo pi, IEnumerable<string> fields, int i, T newItem)
		{
			if (pi != null)
			{
				var o = CheckIfValidElseDefault(fields.ElementAt(i), pi.PropertyType);

				var tc = TypeDescriptor.GetConverter(o.GetType());
				if (tc.CanConvertTo(pi.PropertyType))
				{
					pi.SetValue(newItem, tc.ConvertTo(o, pi.PropertyType), null);
				}
				else
				{
					tc = TypeDescriptor.GetConverter(pi.PropertyType);
					// if we're using a boolean type convertor let's switch to the extended version
					if (tc is BooleanConverter)
						tc = new ExtendedBooleanConvertor();

					pi.SetValue(newItem, tc.CanConvertFrom(o.GetType()) ?
						tc.ConvertFrom(o) : Convert.ChangeType(o, pi.PropertyType, CultureInfo.CurrentCulture), null);
				}
			}
		}

		private static object CheckIfValidElseDefault(object o, Type expected)
		{
			if (String.IsNullOrEmpty(o.ToString()))
			{
				if (expected.IsNumericType())
				{
					// in case we have empty data and it's numeric, let's auto-default to 0
					return 0;
				}
				if (expected.IsBooleanType())
				{
					return false;
				}
			}
			return o;
		}
	}
}
