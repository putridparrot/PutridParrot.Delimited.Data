using System.Collections.Generic;
using System.IO;

namespace Delimited.Data
{
	public interface IDelimitedSeparatedReader
	{
		IList<string> Read(StreamReader reader);
	}
}
