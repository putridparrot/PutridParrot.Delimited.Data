using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace PutridParrot.Delimited.Data.Specializations
{
	/// <summary>
	/// A specialization of a DelimitedStreamReader, set up to handle comma separated value streams
	/// </summary>
	public class CsvReader : DelimitedStreamReader
	{
		public CsvReader(Stream stream)
			: this(stream, null)
		{
		}

		public CsvReader(Stream stream, Encoding encoding) :
			base(new DelimitedSeparatedReader(new CsvOptions()), stream, encoding)
		{
		}

		[ExcludeFromCodeCoverage]
		public CsvReader(string path) :
			this(path, null)
		{
		}

		[ExcludeFromCodeCoverage]
		public CsvReader(string path, Encoding encoding) :
			this(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), encoding)
		{
		}

		public DelimitedOptions Options
		{
			get => ((DelimitedSeparatedReader)DsReader).Options;
            set => ((DelimitedSeparatedReader)DsReader).Options = value;
        }
	}
}
