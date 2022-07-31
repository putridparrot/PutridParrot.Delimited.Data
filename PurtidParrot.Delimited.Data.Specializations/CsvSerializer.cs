using System.Collections.Generic;
using System.IO;

namespace Delimited.Data.Specializations
{
	public static class CsvSerializer<T> where T : new()
	{
		private static IDelimitedSeparatedWriter GetWriter()
		{
			return new DelimitedSeparatedWriter(new CsvOptions());
		}

		private static IDelimitedSeparatedReader GetReader()
		{
			return new DelimitedSeparatedReader(new CsvOptions());
		}

		public static void Serialize(Stream stream, IList<T> list, DelimitedSerializeOptions options)
		{
			DelimitedSerializer<T>.Serialize(GetWriter(), stream, list, options);
		}

		public static IEnumerable<T> Deserialize(Stream stream)
		{
			return DelimitedSerializer<T>.Deserialize(GetReader(), stream);
		}

		public static IEnumerable<T> Deserialize(Stream stream, DelimitedDeserializeOptions options)
		{
			return DelimitedSerializer<T>.Deserialize(GetReader(), stream, options);			
		}

		public static IEnumerable<T> Deserialize(string data)
		{
			return DelimitedSerializer<T>.Deserialize(GetReader(), data);						
		}

		public static IEnumerable<T> Deserialize(string data, DelimitedDeserializeOptions options)
		{
			return DelimitedSerializer<T>.Deserialize(GetReader(), data, options);
		}

		public static IList<FieldReadProperty> GenerateReadMappings(Stream mappingStream)
		{
			return DelimitedSerializer<T>.GenerateReadMappings(mappingStream);
		}

		public static IList<FieldWriteProperty> GenerateWriteMappings(Stream mappingStream)
		{
			return DelimitedSerializer<T>.GenerateWriteMappings(mappingStream);			
		}
	}
}
