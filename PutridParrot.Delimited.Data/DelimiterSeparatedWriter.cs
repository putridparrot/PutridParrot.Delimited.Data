using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

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
			var qualifier = Options.Qualifier == default(char) ? "\"" : Options.Qualifier.ToString(CultureInfo.CurrentCulture);

			return data != null && (Options.QualifyAll || data.IndexOfAny(String.Format("{0}{1}\x0A\x0D", qualifier, Options.Delimiter).ToCharArray()) > -1)
					? qualifier + data.Replace(qualifier, String.Format("{0}{0}", qualifier)) + qualifier : data;
		}

		public void Write(StreamWriter writer, IEnumerable<string> data)
		{
			if (writer == null)
			{
				throw new ArgumentNullException(nameof(writer));
			}
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			var list = new List<string>(data);

			var i = 0;
			var count = list.Count;

			foreach (var item in list)
			{
				writer.Write(Escape(item));
				if (i++ < count - 1)
				{
					writer.Write(Options.Delimiter);
				}
			}
		}

        public async Task WriteAsync(StreamWriter writer, IEnumerable<string> data)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var list = new List<string>(data);

            var i = 0;
            var count = list.Count;

            foreach (var item in list)
            {
                await writer.WriteAsync(Escape(item));
                if (i++ < count - 1)
                {
                    await writer.WriteAsync(Options.Delimiter);
                }
            }
        }

    }

}
