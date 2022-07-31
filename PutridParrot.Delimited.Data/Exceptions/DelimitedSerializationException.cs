using System;
using System.Diagnostics.CodeAnalysis;

namespace PutridParrot.Delimited.Data.Exceptions
{
	[Serializable, ExcludeFromCodeCoverage]
	public class DelimitedSerializationException : Exception
	{
		public DelimitedSerializationException() { }
		public DelimitedSerializationException(string message) : base(message) { }
		public DelimitedSerializationException(string message, Exception inner) : base(message, inner) { }
	}
}
