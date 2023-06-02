using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using PutridParrot.Delimited.Data.Exceptions;

namespace PutridParrot.Delimited.Data
{
	/// <summary>
	/// Gives the capability of reading delimiter separated data. It requires a StreamReader to actual
	/// read data from the stream. The DelimitedSeparatedReader is a low level class which reads a 
	/// line at a time.
	/// </summary>
	public class DelimitedSeparatedReader : IDelimitedSeparatedReader
	{
		public DelimitedSeparatedReader(DelimitedOptions options)
		{
			Options = options;
		}

		public DelimitedOptions Options { get; set; }

		public IList<string> Read(StreamReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException(nameof(reader));
			}

			if (Options == null || Options.Delimiter == default(char))
			{
				throw new DelimitedReaderException("The options need to be supplied and with a delimiter set");
			}

			var line = reader.ReadLine();
			return line == null ? null : Split(line, Options.Delimiter, Options.Qualifier);
        }

        public async Task<IList<string>> ReadAsync(StreamReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (Options == null || Options.Delimiter == default(char))
            {
                throw new DelimitedReaderException("The options need to be supplied and with a delimiter set");
            }

            var line = await reader.ReadLineAsync();
            return line == null ? null : Split(line, Options.Delimiter, Options.Qualifier);
        }

        private static IList<string> Split(string line, char delimiter, char qualifier)
		{
			if (line == null)
				throw new ArgumentNullException(nameof(line));

			// NULL is used to suggest the EOF, so 
			//if (line.StartsWith("\0"))
			//{
			//	return null;
			//}

			var results = new List<string>();
			var position = -1;
			while (position < line.Length)
			{
				results.Add(Parse(line, delimiter, qualifier, ref position));
			}

			return results;
		}

		private static string Parse(string line, char delimiter, char qualifier, ref int startPosition)
		{
			if (startPosition == line.Length - 1)
			{
				startPosition++;
				// the last field is empty
				return String.Empty;
			}

			var fromPos = startPosition + 1;

			// Determine if this is a qualifier field
			if (line[fromPos] == qualifier)
			{
				if (fromPos == line.Length - 1)
				{
					startPosition = line.Length;
					return qualifier.ToString(CultureInfo.CurrentCulture);
				}

				var qualifierAsString = qualifier.ToString(CultureInfo.CurrentCulture);

				var nextSingleQuote = FindQualifier(line, fromPos + 1, qualifier);
				startPosition = nextSingleQuote + 1;
				var extracted = line.Substring(fromPos + 1, nextSingleQuote - fromPos - 1);
				return extracted.Replace($"{qualifierAsString}{qualifierAsString}", qualifierAsString).Trim();
			}

			var nextComma = line.IndexOf(delimiter, fromPos);
			if (nextComma == -1)
			{
				nextComma = startPosition = line.Length;
			}
			else
			{
				startPosition = nextComma;
			}
			return line.Substring(fromPos, nextComma - fromPos).Trim();
		}

		private static int FindQualifier(string data, int startFrom, char qualifier)
		{
			var i = startFrom - 1;
			while (++i < data.Length)
			{
				if (data[i] == qualifier)
				{
					// If this is a double qualifier, bypass the chars
					if (i < data.Length - 1 && (data[i + 1] == qualifier ||
							data[i - 1] == '\\'))
					{
						i++;
						continue;
					}
					return i;
				}
			}
			return i;
		}
	}
}
