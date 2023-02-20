using System;
using System.Diagnostics.CodeAnalysis;

namespace PutridParrot.Delimited.Data.Exceptions
{
	[Serializable, ExcludeFromCodeCoverage]
	public class DelimitedStreamReaderException : Exception
	{
		public DelimitedStreamReaderException() { }
		public DelimitedStreamReaderException(string message) : base(message) { }
		public DelimitedStreamReaderException(string message, Exception inner) : base(message, inner) { }
	}
}
