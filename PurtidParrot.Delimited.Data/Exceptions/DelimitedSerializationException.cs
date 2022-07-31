using System;
using System.Diagnostics.CodeAnalysis;

namespace Delimited.Data.Exceptions
{
	[Serializable, ExcludeFromCodeCoverage]
	public class DelimitedSerializationException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//            

		public DelimitedSerializationException() { }
		public DelimitedSerializationException(string message) : base(message) { }
		public DelimitedSerializationException(string message, Exception inner) : base(message, inner) { }
		protected DelimitedSerializationException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

}
