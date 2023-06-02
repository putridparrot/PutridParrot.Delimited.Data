using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using PutridParrot.Delimited.Data.Exceptions;

namespace PutridParrot.Delimited.Data
{
	/// <summary>
	/// Low-level writer, simply used to write rows of data 
	/// </summary>
	public class DelimitedStreamWriter : IDisposable
	{
		protected Stream Stream;
		protected StreamWriter Writer;
		protected IDelimitedSeparatedWriter DsWriter;
		protected bool Disposed;

		public DelimitedStreamWriter(IDelimitedSeparatedWriter delimiterSeparatedWriter, Stream stream) :
			this(delimiterSeparatedWriter, stream, null)
		{
		}

		public DelimitedStreamWriter(IDelimitedSeparatedWriter delimiterSeparatedWriter, Stream stream, Encoding encoding)
		{
            DsWriter = delimiterSeparatedWriter;
			Stream = stream ?? throw new ArgumentNullException(nameof(stream));
			if (!stream.CanWrite)
			{
				throw new DelimitedStreamWriterException("Unable to write to the supplied stream");
			}
			Writer = encoding != null ? new StreamWriter(stream, encoding) : new StreamWriter(stream);
		}

		[ExcludeFromCodeCoverage]
		public DelimitedStreamWriter(IDelimitedSeparatedWriter delimiterSeparatedWriter, string path) :
			this(delimiterSeparatedWriter, path, null)
		{
		}

		[ExcludeFromCodeCoverage]
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
			if (!Disposed)
			{
				if (disposing)
				{
					Close();
				}
			}
			Disposed = true;
		}

		public void Close()
		{
			if (Writer != null)
			{
				Writer.Close();
				Writer = null;
				Stream = null;
			}
			else if (Stream != null)
			{
				Stream.Close();
				Stream = null;
			}
		}

		public void Flush()
        {
            Writer?.Flush();
        }

		public void Write(IEnumerable<string> data)
		{
			if (Writer == null)
				throw new DelimitedStreamWriterException("StreamWriter is null");

			DsWriter.Write(Writer, data);
		}

		public void WriteLine(IEnumerable<string> data)
		{
			Write(data);
			Writer.WriteLine();
		}

        public async Task WriteAsync(IEnumerable<string> data)
        {
            if (Writer == null)
                throw new DelimitedStreamWriterException("StreamWriter is null");

            await DsWriter.WriteAsync(Writer, data);
        }

        public async Task WriteLineAsync(IEnumerable<string> data)
        {
            await WriteAsync(data);
            await Writer.WriteLineAsync();
        }

    }
}
