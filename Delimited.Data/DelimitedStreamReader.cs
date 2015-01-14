using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Delimited.Data.Exceptions;

namespace Delimited.Data
{
	/// <summary>
	/// Low-level reader, simply used to reading rows of data 
	/// </summary>
	public class DelimitedStreamReader : IDisposable
	{
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
		protected Stream stream;
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
		protected StreamReader reader;
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
		protected IDelimitedSeparatedReader dsReader;
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
		protected bool disposed;

		public DelimitedStreamReader(IDelimitedSeparatedReader delimiterSeparatedReader, Stream stream) :
			this(delimiterSeparatedReader, stream, null)
		{
		}

		public DelimitedStreamReader(IDelimitedSeparatedReader delimiterSeparatedReader, Stream stream, Encoding encoding)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (delimiterSeparatedReader == null)
			{
				throw new ArgumentNullException("delimiterSeparatedReader");
			}

			dsReader = delimiterSeparatedReader;
			this.stream = stream;
			if (!stream.CanRead)
			{
				throw new DelimitedStreamReaderException("Unable to read from the supplied stream");
			}
			reader = (encoding != null) ? new StreamReader(stream, encoding) : new StreamReader(stream);
		}

		[ExcludeFromCodeCoverage]
		public DelimitedStreamReader(IDelimitedSeparatedReader delimiterSeparatedReader, string path) :
			this(delimiterSeparatedReader, path, null)
		{
		}

		[ExcludeFromCodeCoverage]
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		public DelimitedStreamReader(IDelimitedSeparatedReader delimiterSeparatedReader, string path, Encoding encoding) :
			this(delimiterSeparatedReader, new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), encoding)
		{
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					Close();
				}
			}
			disposed = true;
		}

		public void Close()
		{
			if (reader != null)
			{
				reader.Close();
				reader = null;
				stream = null;
			}
			else if (stream != null)
			{
				stream.Close();
				stream = null;
			}
		}

		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "StreamReader")]
		public virtual IList<string> ReadLine()
		{
			if (reader == null)
				throw new DelimitedStreamReaderException("StreamReader is null");

			return dsReader.Read(reader);
		}

		public IList<string> ReadLine(bool ignoreEmptyRows)
		{
			IList<string> line = ReadLine();

			if (ignoreEmptyRows)
			{
				while (line != null && line.All(String.IsNullOrEmpty))
				{
					line = ReadLine();
				}
			}
			return line;
		}
	}

}
