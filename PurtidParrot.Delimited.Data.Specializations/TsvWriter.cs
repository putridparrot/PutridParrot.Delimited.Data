using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Delimited.Data.Specializations
{
	/// <summary>
	/// A specialization of a DelimitedStreamWriter, set up to handle tab separated value streams
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tsv", Justification = "Valid acronym for Tab Separated Value")]
	public class TsvWriter : DelimitedStreamWriter
	{
		public TsvWriter(Stream stream) :
			this(stream, null)
		{
		}

		public TsvWriter(Stream stream, Encoding encoding) :
			base(new DelimitedSeparatedWriter(new TsvOptions()), stream, encoding)
		{
		}

		[ExcludeFromCodeCoverage]
		public TsvWriter(string path) :
			this(path, null)
		{
		}

		[ExcludeFromCodeCoverage]
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		public TsvWriter(string path, Encoding encoding) :
			this(new FileStream(path, FileMode.Create, FileAccess.Write), encoding)
		{
		}

		public TsvOptions Options
		{
			get { return (TsvOptions)((DelimitedSeparatedWriter)dsWriter).Options; }
			set { ((DelimitedSeparatedWriter)dsWriter).Options = value; }
		}
	}

}
