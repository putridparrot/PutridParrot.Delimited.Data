using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace PutridParrot.Delimited.Data.Specializations
{
	/// <summary>
	/// A specialization of a DelimitedStreamReader, set up to handle tab separated value streams
	/// </summary>
	public class TsvReader : DelimitedStreamReader
	{
		public TsvReader(Stream stream)
			: this(stream, null)
		{
		}

		public TsvReader(Stream stream, Encoding encoding) :
			base(new DelimitedSeparatedReader(new TsvOptions()), stream, encoding)
		{
		}

		[ExcludeFromCodeCoverage]
		public TsvReader(string path) :
			this(path, null)
		{
		}

		[ExcludeFromCodeCoverage]
		public TsvReader(string path, Encoding encoding) :
			this(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), encoding)
		{
		}
	}
}
