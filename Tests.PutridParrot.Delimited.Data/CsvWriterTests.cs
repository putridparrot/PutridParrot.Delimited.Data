using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PutridParrot.Delimited.Data;
using PutridParrot.Delimited.Data.Exceptions;
using PutridParrot.Delimited.Data.Specializations;

namespace Tests.PutridParrot.Delimited.Data
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
        public async Task CsvWriter_WriteLineAsync()
        {
            var ms = new MemoryStream();

            var writer = new CsvWriter(ms);
            await writer.WriteLineAsync(new[] { "Hello", "World" });

            writer.Flush();
            ms.Position = 0;

            var reader = new StreamReader(ms);
            var result = await reader.ReadToEndAsync();

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
