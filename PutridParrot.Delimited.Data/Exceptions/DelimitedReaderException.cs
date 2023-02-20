using System;
using System.Diagnostics.CodeAnalysis;

namespace PutridParrot.Delimited.Data.Exceptions
{
	[Serializable, ExcludeFromCodeCoverage]
	public class DelimitedReaderException : Exception
	{
		public DelimitedReaderException() { }
		public DelimitedReaderException(string message) : base(message) { }
		public DelimitedReaderException(string message, Exception inner) : base(message, inner) { }
	}
}
