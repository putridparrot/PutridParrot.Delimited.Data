using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace PutridParrot.Delimited.Data.Specializations
{
	/// <summary>
	/// A specialization of a DelimitedStreamWriter, set up to handle comma separated value streams
	/// </summary>
	public class CsvWriter : DelimitedStreamWriter
	{
		public CsvWriter(Stream stream) :
			this(stream, null)
		{
		}

		public CsvWriter(Stream stream, Encoding encoding) :
			base(new DelimitedSeparatedWriter(new CsvOptions()), stream, encoding)
		{
		}

		[ExcludeFromCodeCoverage]
		public CsvWriter(string path) :
			this(path, null)
		{
		}

		[ExcludeFromCodeCoverage]
		public CsvWriter(string path, Encoding encoding) :
			this(new FileStream(path, FileMode.Create, FileAccess.Write), encoding)
		{
		}

		public DelimitedOptions Options
		{
			get => ((DelimitedSeparatedWriter)DsWriter).Options;
            set => ((DelimitedSeparatedWriter)DsWriter).Options = value;
        }
	}
}
