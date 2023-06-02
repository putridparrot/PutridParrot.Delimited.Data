using System.Collections;
using System.IO;

namespace PutridParrot.Delimited.Data.Specializations
{
	/// <summary>
	/// A Tsv specialization of the DelimitedDataEnumerator
	/// </summary>
	public static class TsvEnumerator
	{
		private static DelimitedSeparatedReader GetReader()
		{
			return new DelimitedSeparatedReader(new TsvOptions());
		}

		public static IEnumerable Deserialize(string data)
		{
			return DelimitedDataEnumerator.Deserialize(GetReader(), data, null);
		}

		public static IEnumerable Deserialize(string data, DelimitedDeserializeOptions? options)
		{
			return DelimitedDataEnumerator.Deserialize(GetReader(), data, options);
		}

		public static IEnumerable Deserialize(Stream stream)
		{
			return DelimitedDataEnumerator.Deserialize(GetReader(), stream, null);
		}

		public static IEnumerable Deserialize(Stream stream, DelimitedDeserializeOptions options)
		{
			return DelimitedDataEnumerator.Deserialize(GetReader(), stream, options);
		}
	}
}
