using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace PutridParrot.Delimited.Data.Tools
{
	public static class DelimitedStreamConnector
	{
		/// <summary>
		/// Pipes an input stream reader to an output stream writer,
		/// i.e. simply reads and writes. This allows us to read in one format
		/// and write to another format.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="output"></param>
		/// <param name="skip">skips n items from the input stream, for example where headers exist that are not required</param>
		public static void Pipe(DelimitedStreamReader input, DelimitedStreamWriter output, int skip = 0)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}

			for (int i = 0; i < skip; i++)
			{
				input.ReadLine();
			}

			IList<string> fields;
			while ((fields = input.ReadLine()) != null)
			{
				output.WriteLine(fields);
			}
			output.Flush();
		}

		[ExcludeFromCodeCoverage]
		public static void Concat(string[] files, DelimitedOptions inputOptions, Encoding inputEncoding, DelimitedStreamWriter output, int skip)
		{
			foreach (string file in files)
			{
				if (File.Exists(file))
				{
					var input = new DelimitedStreamReader(new DelimitedSeparatedReader(inputOptions), File.Open(file, FileMode.Open));
					Pipe(input, output, skip);
				}
			}
		}
	}
}
