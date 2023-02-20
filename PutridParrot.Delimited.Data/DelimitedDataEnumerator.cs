using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PutridParrot.Delimited.Data
{
	/// <summary>
	/// This is a dynamic implementation of the Deserialization methods aimed at use with LINQ
	/// or just generally allows us to interact with the returned objects via properties which map
	/// to header names
	/// </summary>
	/// <example>
	/// </example>
	public static class DelimitedDataEnumerator
	{
		/// <summary>
		/// Allows us to deserialize using the supplied reader and generates an IEnumerable 
		/// of dynamic data.
		/// </summary>
		/// <param name="delimiterSeparatedReader">The delimited reader to use</param>
		/// <param name="data">A string representing delimited data</param>
		/// <returns>An IEnumerable representing the dynamic row data</returns>
		public static IEnumerable Deserialize(IDelimitedSeparatedReader delimiterSeparatedReader, string data)
		{
			return Deserialize(delimiterSeparatedReader, data, null);
		}
		/// <summary>
		/// Allows us to deserialize using the supplied reader and generates an IEnumerable 
		/// of dynamic data.
		/// </summary>
		/// <param name="delimiterSeparatedReader">The delimited reader to use</param>
		/// <param name="data">A string representing delimited data</param>
		/// <param name="options">Options for how the deserializer should handle data</param>
		/// <returns>An IEnumerable representing the dynamic row data</returns>
		public static IEnumerable Deserialize(IDelimitedSeparatedReader delimiterSeparatedReader, string data, DelimitedDeserializeOptions options)
		{
			using (var memoryStream = new MemoryStream())
			{
				memoryStream.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
				memoryStream.Seek(0, SeekOrigin.Begin);

				foreach (var line in Deserialize(delimiterSeparatedReader, memoryStream, options))
				{
					yield return line;
				}
			}
		}
		/// <summary>
		/// Allows us to deserialize using the supplied reader and generates an IEnumerable 
		/// of dynamic data.
		/// </summary>
		/// <param name="delimiterSeparatedReader">The delimited reader to use</param>
		/// <param name="stream">A stream representing the delimited data</param>
		/// <returns>An IEnumerable representing the dynamic row data</returns>
		public static IEnumerable Deserialize(IDelimitedSeparatedReader delimiterSeparatedReader, Stream stream)
		{
			return Deserialize(delimiterSeparatedReader, stream, null);
		}

		private static string[] CreateColumnHeadings(int length)
		{
			var headers = new string[length];
			for (var i = 1; i <= length; i++)
			{
				headers[i - 1] = "Column" + i;
			}
			return headers;
		}
		/// <summary>
		/// Allows us to deserialize using the supplied reader and generates an IEnumerable 
		/// of dynamic data.
		/// </summary>
		/// <param name="delimiterSeparatedReader">The delimited reader to use</param>
		/// <param name="stream">A stream representing the delimited data</param>
		/// <param name="options">Options for how the deserializer should handle data</param>
		/// <returns>An IEnumerable representing the dynamic row data</returns>
		public static IEnumerable Deserialize(IDelimitedSeparatedReader delimiterSeparatedReader, Stream stream, DelimitedDeserializeOptions options)
		{
			using (var reader = new DelimitedStreamReader(delimiterSeparatedReader, stream))
			{
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
					IEnumerable<string> fields;
					while ((fields = reader.ReadLine()) != null)
					{
						var f = fields.ToArray();
						yield return new DelimitedRow(CreateColumnHeadings(f.Length), f);
					}
				}
				else
				{
					IEnumerable<string> headings = reader.ReadLine();
					if (headings != null)
					{
						var columnHeadings = headings.ToArray();

						IEnumerable<string> fields;
						while ((fields = reader.ReadLine()) != null)
						{
							if (options != null && options.IgnoreEmptyRows)
							{
								if (fields.All(String.IsNullOrEmpty))
									continue;
							}

							yield return new DelimitedRow(columnHeadings, fields.ToArray());
						}
					}
				}
			}
		}
	}
}
