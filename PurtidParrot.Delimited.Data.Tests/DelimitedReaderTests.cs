using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using Delimited.Data.Exceptions;

namespace Delimited.Data.Tests
{
	[ExcludeFromCodeCoverage]
	public class DelimitedReaderTests
	{
		[Test]
		public void NullDelimitedReader()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				using (var reader = new DelimitedStreamReader(null, Utils.ToStream("One|Two|Three")))
				{
				}
			});
		}

		[Test]
		public void NullDelimitedOptionsShouldException()
		{
			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(null), Utils.ToStream("One|Two|Three")))
			{
				Assert.Throws<DelimitedReaderException>(() => reader.ReadLine());
			}
		}

		[Test]
		public void OneLinePipeDelimitedTest()
		{
			var options = new DelimitedOptions('|');

			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(options), Utils.ToStream("One|Two|Three")))
			{
				IEnumerable<string> results = reader.ReadLine();
				Assert.AreEqual(3, results.Count());
				Assert.AreEqual("One", results.ElementAt(0));
				Assert.AreEqual("Two", results.ElementAt(1));
				Assert.AreEqual("Three", results.ElementAt(2));
			}
		}

		[Test]
		public void MultiLinePipeDelimitedTest()
		{
			var options = new DelimitedOptions('|');

			const string data = "One|Two|Three\r\nFour|Five|Six\r\nSeven|Eight|Nine";

			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(options), Utils.ToStream(data)))
			{
				IEnumerable<string> results = reader.ReadLine();
				Assert.AreEqual(3, results.Count());
				Assert.AreEqual("One", results.ElementAt(0));
				Assert.AreEqual("Two", results.ElementAt(1));
				Assert.AreEqual("Three", results.ElementAt(2));

				results = reader.ReadLine();
				Assert.AreEqual(3, results.Count());
				Assert.AreEqual("Four", results.ElementAt(0));
				Assert.AreEqual("Five", results.ElementAt(1));
				Assert.AreEqual("Six", results.ElementAt(2));

				results = reader.ReadLine();
				Assert.AreEqual(3, results.Count());
				Assert.AreEqual("Seven", results.ElementAt(0));
				Assert.AreEqual("Eight", results.ElementAt(1));
				Assert.AreEqual("Nine", results.ElementAt(2));
			}
		}

		[Test]
		public void MultiLinePipeDelimitedTest_DifferentNewLineStyles()
		{
			var options = new DelimitedOptions('|');

			const string data = "One|Two|Three\rFour|Five|Six\nSeven|Eight|Nine";

			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(options), Utils.ToStream(data)))
			{
				IEnumerable<string> results = reader.ReadLine();
				Assert.AreEqual(3, results.Count());
				Assert.AreEqual("One", results.ElementAt(0));
				Assert.AreEqual("Two", results.ElementAt(1));
				Assert.AreEqual("Three", results.ElementAt(2));

				results = reader.ReadLine();
				Assert.AreEqual(3, results.Count());
				Assert.AreEqual("Four", results.ElementAt(0));
				Assert.AreEqual("Five", results.ElementAt(1));
				Assert.AreEqual("Six", results.ElementAt(2));

				results = reader.ReadLine();
				Assert.AreEqual(3, results.Count());
				Assert.AreEqual("Seven", results.ElementAt(0));
				Assert.AreEqual("Eight", results.ElementAt(1));
				Assert.AreEqual("Nine", results.ElementAt(2));
			}
		}

		[Test]
		public void MultiLinePipeDelimitedTest_DifferentNumberOfItems()
		{
			var options = new DelimitedOptions('|');

			const string data = "One|Two|Three\nFour|Six\nSeven|Eight";

			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(options), Utils.ToStream(data)))
			{
				IEnumerable<string> results = reader.ReadLine();
				Assert.AreEqual(3, results.Count());
				Assert.AreEqual("One", results.ElementAt(0));
				Assert.AreEqual("Two", results.ElementAt(1));
				Assert.AreEqual("Three", results.ElementAt(2));

				results = reader.ReadLine();
				Assert.AreEqual(2, results.Count());
				Assert.AreEqual("Four", results.ElementAt(0));
				Assert.AreEqual("Six", results.ElementAt(1));

				results = reader.ReadLine();
				Assert.AreEqual(2, results.Count());
				Assert.AreEqual("Seven", results.ElementAt(0));
				Assert.AreEqual("Eight", results.ElementAt(1));
			}
		}

		[Test]
		public void EmbeddedPipe_UsingSpeechMarksAsQualifierWithSingleItem()
		{
			var options = new DelimitedOptions('|');

			const string data = "\"|One|\"";

			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(options), Utils.ToStream(data)))
			{
				IEnumerable<string> results = reader.ReadLine();
				Assert.AreEqual(1, results.Count());
				Assert.AreEqual("|One|", results.ElementAt(0));
			}
		}

		[Test]
		public void EmbeddedPipe_UsingSpeechMarksAsQualifier()
		{
			var options = new DelimitedOptions('|');

			const string data = "One|Two|Three|\"|Four|\"";

			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(options), Utils.ToStream(data)))
			{
				IEnumerable<string> results = reader.ReadLine();
				Assert.AreEqual(4, results.Count());
				Assert.AreEqual("One", results.ElementAt(0));
				Assert.AreEqual("Two", results.ElementAt(1));
				Assert.AreEqual("Three", results.ElementAt(2));
				Assert.AreEqual("|Four|", results.ElementAt(3));
			}
		}

		[Test]
		public void ReadLine_WithIgnoreEmptyRows()
		{
			var options = new DelimitedOptions('|');

			const string data = "\n\nOne|Two|Three\n\n\nFour|Five|Six\nSeven|Eight|Nine\n\n";

			using (var reader = new DelimitedStreamReader(new DelimitedSeparatedReader(options),
					Utils.ToStream(data)))
			{
				IEnumerable<string> results = reader.ReadLine(true);
				Assert.AreEqual(3, results.Count());
				Assert.AreEqual("One", results.ElementAt(0));
				Assert.AreEqual("Two", results.ElementAt(1));
				Assert.AreEqual("Three", results.ElementAt(2));

				results = reader.ReadLine(true);
				Assert.AreEqual(3, results.Count());
				Assert.AreEqual("Four", results.ElementAt(0));
				Assert.AreEqual("Five", results.ElementAt(1));
				Assert.AreEqual("Six", results.ElementAt(2));

				results = reader.ReadLine(true);
				Assert.AreEqual(3, results.Count());
				Assert.AreEqual("Seven", results.ElementAt(0));
				Assert.AreEqual("Eight", results.ElementAt(1));
				Assert.AreEqual("Nine", results.ElementAt(2));
			}
		}
	}

}
