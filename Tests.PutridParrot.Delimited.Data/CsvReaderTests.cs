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
	public class CsvDataReaderTests
	{
		[Test]
		public void CsvReader_Ctor_NonReadableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(false);

			Assert.Throws<DelimitedStreamReaderException>(() => { var _ = new CsvReader(mock.Object); });
		}

		[Test]
		public void CsvReader_Ctor2_NonReadableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(false);

			Assert.Throws<DelimitedStreamReaderException>(() => { var _ = new CsvReader(mock.Object, Encoding.ASCII); });
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

			var csv = reader.ReadLine();

			Assert.AreEqual(2, csv.Count);
			Assert.AreEqual("Hello", csv[0]);
			Assert.AreEqual("World", csv[1]);
		}

        [Test]
        public async Task CsvReader_ReadLineAsync_MinimalTest()
        {
            var reader = new CsvReader(Utils.ToStream("Hello, World"));

            var csv = await reader.ReadLineAsync();

            Assert.AreEqual(2, csv.Count);
            Assert.AreEqual("Hello", csv[0]);
            Assert.AreEqual("World", csv[1]);
        }

        [Test]
        public void CsvReader_ReadLine_EmptyData()
        {
            var reader = new CsvReader(Utils.ToStream(""));

            var csv = reader.ReadLine();

			Assert.IsNull(csv);
        }

        [Test]
        public async Task CsvReader_ReadLineAsync_EmptyData()
        {
            var reader = new CsvReader(Utils.ToStream(""));

            var csv = await reader.ReadLineAsync();

            Assert.IsNull(csv);
        }

        [Test]
		public void CsvReader_ReadOneItemWithQuotes()
		{
			var reader = new CsvReader(Utils.ToStream("\"Hello, World\""));
			var csv = reader.ReadLine();

			Assert.AreEqual(1, csv.Count);
			Assert.AreEqual("Hello, World", csv[0]);
		}

        [Test]
        public async Task CsvReader_ReadAsyncOneItemWithQuotes()
        {
            var reader = new CsvReader(Utils.ToStream("\"Hello, World\""));
            var csv = await reader.ReadLineAsync();

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
