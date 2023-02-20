using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using PutridParrot.Delimited.Data.Exceptions;
using PutridParrot.Delimited.Data.Specializations;
using Moq;
using NUnit.Framework;

namespace PutridParrot.Delimited.Data.Tests
{
	[ExcludeFromCodeCoverage]
	public class CsvDataReaderTests
	{
		[Test]
		public void CsvReader_Ctor_NonReadableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(false);

			Assert.Throws<DelimitedStreamReaderException>(() => { CsvReader reader = new CsvReader(mock.Object); });
		}

		[Test]
		public void CsvReader_Ctor2_NonReadableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(false);

			Assert.Throws<DelimitedStreamReaderException>(() => { CsvReader reader = new CsvReader(mock.Object, Encoding.ASCII); });
		}

		[Test]
		public void CsvReader_CloseImplicitlyCalledViaDispose()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(true);
			mock.Setup(s => s.Close()).Verifiable();

			var reader = new CsvReader(mock.Object);
			reader.Dispose();

			mock.Verify();
		}

		[Test]
		public void CsvReader_Close_ShouldCloseStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(true);
			mock.Setup(s => s.Close()).Verifiable();

			var reader = new CsvReader(mock.Object);
			reader.Close();

			mock.Verify();
		}

		[Test]
		public void CsvReader_ReadLine_MinimalTest()
		{
			var reader = new CsvReader(Utils.ToStream("Hello, World"));

			IList<string> csv = reader.ReadLine();

			Assert.AreEqual(2, csv.Count);
			Assert.AreEqual("Hello", csv[0]);
			Assert.AreEqual("World", csv[1]);
		}

		[Test]
		public void CsvReader_ReadOneItemWithQuotes()
		{
			var reader = new CsvReader(Utils.ToStream("\"Hello, World\""));
			IList<string> csv = reader.ReadLine();

			Assert.AreEqual(1, csv.Count);
			Assert.AreEqual("Hello, World", csv[0]);
		}

		[Test]
		public void CsvReader_CheckOptionsSetterGetter()
		{
			var options = new DelimitedOptions('.');

			var reader = new CsvReader(Utils.ToStream("\"Hello, World\""))
			{
				Options = options
			};

			Assert.AreEqual(options, reader.Options);
		}
	}
}
