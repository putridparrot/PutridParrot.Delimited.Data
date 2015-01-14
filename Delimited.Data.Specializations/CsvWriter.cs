using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Delimited.Data.Specializations
{
	/// <summary>
	/// A specialization of a DelimitedStreamWriter, set up to handle comma separated value streams
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Csv", Justification = "Valid acronym for Comma Separated Value")]
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
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		public CsvWriter(string path, Encoding encoding) :
			this(new FileStream(path, FileMode.Create, FileAccess.Write), encoding)
		{
		}

		public DelimitedOptions Options
		{
			get { return ((DelimitedSeparatedWriter)dsWriter).Options; }
			set { ((DelimitedSeparatedWriter)dsWriter).Options = value; }
		}
	}

}
