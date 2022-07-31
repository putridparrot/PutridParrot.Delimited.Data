using System.Collections.Generic;
using System.IO;

namespace Delimited.Data
{
	public interface IDelimitedSeparatedWriter
	{
		void Write(StreamWriter writer, IEnumerable<string> data);
	}
}
