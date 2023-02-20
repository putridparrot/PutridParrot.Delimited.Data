﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using PutridParrot.Delimited.Data.Exceptions;

namespace PutridParrot.Delimited.Data
{
	/// <summary>
	/// Low-level reader, simply used to reading rows of data 
	/// </summary>
	public class DelimitedStreamReader : IDisposable
	{
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
		protected Stream Stream;
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
		protected StreamReader Reader;
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
		protected IDelimitedSeparatedReader DsReader;
		[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
		protected bool Disposed;

		public DelimitedStreamReader(IDelimitedSeparatedReader delimiterSeparatedReader, Stream stream) :
			this(delimiterSeparatedReader, stream, null)
		{
		}

		public DelimitedStreamReader(IDelimitedSeparatedReader delimiterSeparatedReader, Stream stream, Encoding encoding)
		{
            DsReader = delimiterSeparatedReader ?? throw new ArgumentNullException(nameof(delimiterSeparatedReader));
			Stream = stream ?? throw new ArgumentNullException(nameof(stream));
			if (!stream.CanRead)
			{
				throw new DelimitedStreamReaderException("Unable to read from the supplied stream");
			}
			Reader = encoding != null ? new StreamReader(stream, encoding) : new StreamReader(stream);
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
			if (Reader != null)
			{
				Reader.Close();
				Reader = null;
				Stream = null;
			}
			else if (Stream != null)
			{
				Stream.Close();
				Stream = null;
			}
		}

		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "StreamReader")]
		public virtual IList<string> ReadLine()
		{
			if (Reader == null)
				throw new DelimitedStreamReaderException("StreamReader is null");

			return DsReader.Read(Reader);
		}

		public IList<string> ReadLine(bool ignoreEmptyRows)
		{
			var line = ReadLine();

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
