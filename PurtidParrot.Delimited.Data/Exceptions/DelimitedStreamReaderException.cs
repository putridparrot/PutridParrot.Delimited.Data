using System;
using System.Diagnostics.CodeAnalysis;

namespace PutridParrot.Delimited.Data.Exceptions
{
	[Serializable, ExcludeFromCodeCoverage]
	public class DelimitedStreamReaderException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public DelimitedStreamReaderException() { }
		public DelimitedStreamReaderException(string message) : base(message) { }
		public DelimitedStreamReaderException(string message, Exception inner) : base(message, inner) { }
		protected DelimitedStreamReaderException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

}
