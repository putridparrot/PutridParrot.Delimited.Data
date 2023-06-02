using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PutridParrot.Delimited.Data
{
	public interface IDelimitedSeparatedReader
	{
		IList<string> Read(StreamReader reader);
        Task<IList<string>> ReadAsync(StreamReader reader);
	}
}
