using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PutridParrot.Delimited.Data
{
	public interface IDelimitedSeparatedWriter
	{
		void Write(StreamWriter writer, IEnumerable<string?> data);
        Task WriteAsync(StreamWriter writer, IEnumerable<string> data);
	}
}
