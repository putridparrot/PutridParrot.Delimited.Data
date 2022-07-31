using System.Collections.Generic;
using System.IO;

namespace PutridParrot.Delimited.Data
{
	public interface IDelimitedSeparatedReader
	{
		IList<string> Read(StreamReader reader);
	}
}
