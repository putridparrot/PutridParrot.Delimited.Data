using System;
using System.Diagnostics.CodeAnalysis;

namespace PutridParrot.Delimited.Data.Exceptions
{
	[Serializable, ExcludeFromCodeCoverage]
	public class DelimitedStreamWriterException : Exception
	{
		public DelimitedStreamWriterException() { }
		public DelimitedStreamWriterException(string message) : base(message) { }
		public DelimitedStreamWriterException(string message, Exception inner) : base(message, inner) { }
	}
}
