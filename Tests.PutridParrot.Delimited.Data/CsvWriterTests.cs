using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security;
using System.Text;
using PutridParrot.Delimited.Data.Exceptions;
using PutridParrot.Delimited.Data.Specializations;
using Moq;
using NUnit.Framework;

namespace PutridParrot.Delimited.Data.Tests
{
	[ExcludeFromCodeCoverage]
	public class CsvWriterTests
	{
		[Test]
		public void CsvWriter_Ctor_NonWritableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanWrite).Returns(false);

			Assert.Throws<DelimitedStreamWriterException>(() => { var _ = new CsvWriter(mock.Object); });
		}

		[Test]
		public void CsvWriter_Ctor2_NonWriteableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanWrite).Returns(false);

			Assert.Throws<DelimitedStreamWriterException>(() => { var _ = new CsvWriter(mock.Object, Encoding.ASCII); });
		}

		[Test]
		public void CsvWriter_CloseImplicitlyCalledViaDispose()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanWrite).Returns(true);
			mock.Setup(s => s.Close()).Verifiable();

			var writer = new CsvWriter((mock.Object));
			writer.Dispose();

			mock.Verify();
		}

		[Test]
		public void CsvWriter_Close_ShouldCloseStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanWrite).Returns(true);
			mock.Setup(s => s.Close()).Verifiable();

			var writer = new CsvWriter(mock.Object);
			writer.Close();

			mock.Verify();
		}

		[Test]
		public void CsvWriter_WriteLine()
		{
			var ms = new MemoryStream();

			var writer = new CsvWriter(ms);
			writer.WriteLine(new[] { "Hello", "World" });

			writer.Flush();
			ms.Position = 0;

			var reader = new StreamReader(ms);
			var result = reader.ReadToEnd();

			Assert.AreEqual($"Hello,World{Environment.NewLine}", result);
		}

        [Test]
		public void CsvReader_CheckOptionsSetterGetter()
		{
			var options = new DelimitedOptions('.');

			var writer = new CsvWriter(new MemoryStream()) {Options = options};

			Assert.AreEqual(options, writer.Options);
		}
	}

}
