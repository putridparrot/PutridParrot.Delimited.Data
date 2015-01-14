using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Delimited.Data.Exceptions;

namespace Delimited.Data.Tests
{
	[ExcludeFromCodeCoverage]
	public class DelimitedReaderTests
	{
		[Fact]
		public void NullDelimitedReader()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				using (var reader = new DelimitedStreamReader(null, Utils.ToStream("One|Two|Three")))
				{
				}
			});
		}

		[Fact]
		public void NullDelimitedOptionsShouldException()
		{
			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(null), Utils.ToStream("One|Two|Three")))
			{
				Assert.Throws<DelimitedReaderException>(() => reader.ReadLine());
			}
		}

		[Fact]
		public void OneLinePipeDelimitedTest()
		{
			var options = new DelimitedOptions('|');

			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(options), Utils.ToStream("One|Two|Three")))
			{
				IEnumerable<string> results = reader.ReadLine();
				Assert.Equal(3, results.Count());
				Assert.Equal("One", results.ElementAt(0));
				Assert.Equal("Two", results.ElementAt(1));
				Assert.Equal("Three", results.ElementAt(2));
			}
		}

		[Fact]
		public void MultiLinePipeDelimitedTest()
		{
			var options = new DelimitedOptions('|');

			const string data = "One|Two|Three\r\nFour|Five|Six\r\nSeven|Eight|Nine";

			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(options), Utils.ToStream(data)))
			{
				IEnumerable<string> results = reader.ReadLine();
				Assert.Equal(3, results.Count());
				Assert.Equal("One", results.ElementAt(0));
				Assert.Equal("Two", results.ElementAt(1));
				Assert.Equal("Three", results.ElementAt(2));

				results = reader.ReadLine();
				Assert.Equal(3, results.Count());
				Assert.Equal("Four", results.ElementAt(0));
				Assert.Equal("Five", results.ElementAt(1));
				Assert.Equal("Six", results.ElementAt(2));

				results = reader.ReadLine();
				Assert.Equal(3, results.Count());
				Assert.Equal("Seven", results.ElementAt(0));
				Assert.Equal("Eight", results.ElementAt(1));
				Assert.Equal("Nine", results.ElementAt(2));
			}
		}

		[Fact]
		public void MultiLinePipeDelimitedTest_DifferentNewLineStyles()
		{
			var options = new DelimitedOptions('|');

			const string data = "One|Two|Three\rFour|Five|Six\nSeven|Eight|Nine";

			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(options), Utils.ToStream(data)))
			{
				IEnumerable<string> results = reader.ReadLine();
				Assert.Equal(3, results.Count());
				Assert.Equal("One", results.ElementAt(0));
				Assert.Equal("Two", results.ElementAt(1));
				Assert.Equal("Three", results.ElementAt(2));

				results = reader.ReadLine();
				Assert.Equal(3, results.Count());
				Assert.Equal("Four", results.ElementAt(0));
				Assert.Equal("Five", results.ElementAt(1));
				Assert.Equal("Six", results.ElementAt(2));

				results = reader.ReadLine();
				Assert.Equal(3, results.Count());
				Assert.Equal("Seven", results.ElementAt(0));
				Assert.Equal("Eight", results.ElementAt(1));
				Assert.Equal("Nine", results.ElementAt(2));
			}
		}

		[Fact]
		public void MultiLinePipeDelimitedTest_DifferentNumberOfItems()
		{
			var options = new DelimitedOptions('|');

			const string data = "One|Two|Three\nFour|Six\nSeven|Eight";

			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(options), Utils.ToStream(data)))
			{
				IEnumerable<string> results = reader.ReadLine();
				Assert.Equal(3, results.Count());
				Assert.Equal("One", results.ElementAt(0));
				Assert.Equal("Two", results.ElementAt(1));
				Assert.Equal("Three", results.ElementAt(2));

				results = reader.ReadLine();
				Assert.Equal(2, results.Count());
				Assert.Equal("Four", results.ElementAt(0));
				Assert.Equal("Six", results.ElementAt(1));

				results = reader.ReadLine();
				Assert.Equal(2, results.Count());
				Assert.Equal("Seven", results.ElementAt(0));
				Assert.Equal("Eight", results.ElementAt(1));
			}
		}

		[Fact]
		public void EmbeddedPipe_UsingSpeechMarksAsQualifierWithSingleItem()
		{
			var options = new DelimitedOptions('|');

			const string data = "\"|One|\"";

			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(options), Utils.ToStream(data)))
			{
				IEnumerable<string> results = reader.ReadLine();
				Assert.Equal(1, results.Count());
				Assert.Equal("|One|", results.ElementAt(0));
			}
		}

		[Fact]
		public void EmbeddedPipe_UsingSpeechMarksAsQualifier()
		{
			var options = new DelimitedOptions('|');

			const string data = "One|Two|Three|\"|Four|\"";

			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(options), Utils.ToStream(data)))
			{
				IEnumerable<string> results = reader.ReadLine();
				Assert.Equal(4, results.Count());
				Assert.Equal("One", results.ElementAt(0));
				Assert.Equal("Two", results.ElementAt(1));
				Assert.Equal("Three", results.ElementAt(2));
				Assert.Equal("|Four|", results.ElementAt(3));
			}
		}

		[Fact]
		public void ReadLine_WithIgnoreEmptyRows()
		{
			var options = new DelimitedOptions('|');

			const string data = "\n\nOne|Two|Three\n\n\nFour|Five|Six\nSeven|Eight|Nine\n\n";

			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(options),
					Utils.ToStream(data)))
			{
				IEnumerable<string> results = reader.ReadLine(true);
				Assert.Equal(3, results.Count());
				Assert.Equal("One", results.ElementAt(0));
				Assert.Equal("Two", results.ElementAt(1));
				Assert.Equal("Three", results.ElementAt(2));

				results = reader.ReadLine(true);
				Assert.Equal(3, results.Count());
				Assert.Equal("Four", results.ElementAt(0));
				Assert.Equal("Five", results.ElementAt(1));
				Assert.Equal("Six", results.ElementAt(2));

				results = reader.ReadLine(true);
				Assert.Equal(3, results.Count());
				Assert.Equal("Seven", results.ElementAt(0));
				Assert.Equal("Eight", results.ElementAt(1));
				Assert.Equal("Nine", results.ElementAt(2));
			}
		}
	}

}
