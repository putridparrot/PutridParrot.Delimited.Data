using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using PutridParrot.Delimited.Data.Exceptions;

namespace PutridParrot.Delimited.Data
{
	/// <summary>
	/// Low-level writer, simply used to write rows of data 
	/// </summary>
	public class DelimitedStreamWriter : IDisposable
	{
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
		protected Stream stream;
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
		protected StreamWriter writer;
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
		protected IDelimitedSeparatedWriter dsWriter;
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
		protected bool disposed;

		public DelimitedStreamWriter(IDelimitedSeparatedWriter delimiterSeparatedWriter, Stream stream) :
			this(delimiterSeparatedWriter, stream, null)
		{
		}

		public DelimitedStreamWriter(IDelimitedSeparatedWriter delimiterSeparatedWriter, Stream stream, Encoding encoding)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}

			dsWriter = delimiterSeparatedWriter;
			this.stream = stream;
			if (!stream.CanWrite)
			{
				throw new DelimitedStreamWriterException("Unable to write to the supplied stream");
			}
			writer = (encoding != null) ? new StreamWriter(stream, encoding) : new StreamWriter(stream);
		}

		[ExcludeFromCodeCoverage]
		public DelimitedStreamWriter(IDelimitedSeparatedWriter delimiterSeparatedWriter, string path) :
			this(delimiterSeparatedWriter, path, null)
		{
		}

		[ExcludeFromCodeCoverage]
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		public DelimitedStreamWriter(IDelimitedSeparatedWriter delimiterSeparatedWriter, string path, Encoding encoding) :
			this(delimiterSeparatedWriter, new FileStream(path, FileMode.Create, FileAccess.Write), encoding)
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
			if (writer != null)
			{
				writer.Close();
				writer = null;
				stream = null;
			}
			else if (stream != null)
			{
				stream.Close();
				stream = null;
			}
		}

		public void Flush()
		{
			if (writer != null)
			{
				writer.Flush();
			}
		}

		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "StreamWriter")]
		public void Write(IEnumerable<string> data)
		{
			if (writer == null)
				throw new DelimitedStreamWriterException("StreamWriter is null");

			dsWriter.Write(writer, data);
		}

		public void WriteLine(IEnumerable<string> data)
		{
			Write(data);
			writer.WriteLine();
		}
	}

}
