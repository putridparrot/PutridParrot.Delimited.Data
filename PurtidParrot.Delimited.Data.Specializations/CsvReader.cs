using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Delimited.Data.Specializations
{
	/// <summary>
	/// A specialization of a DelimitedStreamReader, set up to handle comma separated value streams
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Csv", Justification = "Valid acronym for Comma Separated Value")]
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
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		public CsvReader(string path, Encoding encoding) :
			this(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), encoding)
		{
		}

		public DelimitedOptions Options
		{
			get { return ((DelimitedSeparatedReader)dsReader).Options; }
			set { ((DelimitedSeparatedReader)dsReader).Options = value; }
		}
	}

}
