using System;
using System.Diagnostics.CodeAnalysis;

namespace Delimited.Data.Exceptions
{
	[Serializable, ExcludeFromCodeCoverage]
	public class DelimitedStreamWriterException : Exception
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public DelimitedStreamWriterException() { }
		public DelimitedStreamWriterException(string message) : base(message) { }
		public DelimitedStreamWriterException(string message, Exception inner) : base(message, inner) { }
		protected DelimitedStreamWriterException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

}
