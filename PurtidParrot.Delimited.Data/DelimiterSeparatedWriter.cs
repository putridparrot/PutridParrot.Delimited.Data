using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace PutridParrot.Delimited.Data
{
	/// <summary>
	/// Gives the capability of writing delimiter separated data. It requires a StreamWriter to actual
	/// write data to the stream.
	/// </summary>
	public class DelimitedSeparatedWriter : IDelimitedSeparatedWriter
	{
		public DelimitedSeparatedWriter(DelimitedOptions options)
		{
			Options = options;
		}

		public DelimitedOptions Options { get; set; }

		private string Escape(string data)
		{
			string qualifier = Options.Qualifier == default(char) ? "\"" : Options.Qualifier.ToString(CultureInfo.CurrentCulture);

			return data != null && (Options.QualifyAll || data.IndexOfAny(String.Format("{0}{1}\x0A\x0D", qualifier, Options.Delimiter).ToCharArray()) > -1)
					? qualifier + data.Replace(qualifier, String.Format("{0}{0}", qualifier)) + qualifier : data;
		}

		public void Write(StreamWriter writer, IEnumerable<string> data)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}

			IList<string> list = new List<string>(data);

			int i = 0;
			int count = list.Count;

			foreach (string item in list)
			{
				writer.Write(Escape(item));
				if (i++ < count - 1)
				{
					writer.Write(Options.Delimiter);
				}
			}
		}
	}

}
