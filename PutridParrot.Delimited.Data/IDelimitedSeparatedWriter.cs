using System.Collections.Generic;
using System.IO;

namespace PutridParrot.Delimited.Data
{
	public interface IDelimitedSeparatedWriter
	{
		void Write(StreamWriter writer, IEnumerable<string> data);
	}
}
