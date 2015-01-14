using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Delimited.Data;

namespace DelimitedCodeGenerator
{
	public class Parser
	{
		public static string Parse(string inputFilename, bool expectHeader, char delimiter, char qualifier, bool qualifyAll, ICodeBuilder builder)
		{
			if (inputFilename == null)
				throw new ArgumentNullException("inputFilename");
			if (builder == null)
				throw new ArgumentNullException("builder");
			if (!File.Exists(inputFilename))
				throw new FileNotFoundException("File does not exist", inputFilename);

			using (var istream = File.OpenRead(inputFilename))
			{
				return Parse(istream, expectHeader, delimiter, qualifier, qualifyAll, builder);
			}
		}

		public static string Parse(Stream istream, bool expectHeader, char delimiter, char qualifier, bool qualifyAll, ICodeBuilder builder)
		{
			if (istream == null)
				throw new ArgumentNullException("istream");
			if (builder == null)
				throw new ArgumentNullException("builder");

			using (var reader = new DelimitedStreamReader(
				new DelimitedSeparatedReader(
					new DelimitedOptions(delimiter, qualifier, qualifyAll)), istream))
			{
				var line = reader.ReadLine(true);
				if (line != null)
				{
					HeadingType[] headings = GenerateHeaders(line, expectHeader);
					DetermineColumnTypes(reader, headings);
					return builder.Generate(headings, expectHeader);
				}
			}
			return null;
		}


		private static HeadingType[] GenerateHeaders(IList<string> line, bool expectHeader)
		{
			var headings = new HeadingType[line.Count()];

			if (expectHeader)
			{
				int i = 0;
				foreach (var heading in line)
				{
					headings[i++] = new HeadingType { Heading = heading };
				}
			}
			else
			{
				for (var i = 0; i < headings.Length; i++)
				{
					headings[i] = new HeadingType { Heading = "Column" + i };
				}
			}
			return headings;
		}

		private static void DetermineColumnTypes(DelimitedStreamReader reader, HeadingType[] headingTypes)
		{
			const int SAMPLE_ROWS = 10;

			var inferences = headingTypes.ToDictionary(ht => ht, ht => new List<Type>());

			int rowCount = 0;
			IEnumerable<string> line;
			while ((line = reader.ReadLine(true)) != null && rowCount < SAMPLE_ROWS)
			{
				var asArray = line.ToArray();

				for (var i = 0; i < headingTypes.Length; i++)
				{
					if (i < asArray.Length)
					{
						inferences[headingTypes[i]].Add(InferType(asArray[i]));
					}
				}

				rowCount++;
			}

			// now try to determine types from previous inferences
			foreach (var infer in inferences)
			{
				HeadingType ht = infer.Key;
				ht.Type = (infer.Value.Count > 0 && infer.Value.TrueForAll(t => t == infer.Value[0])) ?
					infer.Value[0] : typeof(string);
			}
		}

		private static Type InferType(string value)
		{
			bool b;
			if (bool.TryParse(value, out b))
				return b.GetType();

			int i;
			if (int.TryParse(value, out i))
				return i.GetType();

			uint ui;
			if (uint.TryParse(value, out ui))
				return ui.GetType();

			long l;
			if (long.TryParse(value, out l))
				return l.GetType();

			ulong ul;
			if (ulong.TryParse(value, out ul))
				return ul.GetType();

			float f;
			if (float.TryParse(value, out f))
				return f.GetType();

			double d;
			if (Double.TryParse(value, out d))
				return d.GetType();

			decimal dec;
			if (decimal.TryParse(value, out dec))
				return dec.GetType();

			DateTime dt;
			if (DateTime.TryParse(value, out dt))
				return dt.GetType();

			return typeof(string);
		}
	}

}
