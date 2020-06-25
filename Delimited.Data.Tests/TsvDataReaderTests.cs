using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Delimited.Data.Exceptions;
using Delimited.Data.Specializations;
using Moq;
using NUnit.Framework;

namespace Delimited.Data.Tests
{
	[ExcludeFromCodeCoverage]
	public class TsvDataReaderTests
	{
		[Test]
		public void TsvReader_Ctor_NonReadableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(false);

			Assert.Throws<DelimitedStreamReaderException>(() => { TsvReader reader = new TsvReader(mock.Object); });
		}

		[Test]
		public void TsvReader_Ctor2_NonReadableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(false);

			Assert.Throws<DelimitedStreamReaderException>(() => { TsvReader reader = new TsvReader(mock.Object, Encoding.ASCII); });
		}

		[Test]
		public void TsvReader_CloseImplicitlyCalledViaDispose()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(true);
			mock.Setup(s => s.Close()).Verifiable();

			var reader = new TsvReader(mock.Object);
			reader.Dispose();

			mock.Verify();
		}

		[Test]
		public void TsvReader_Close_ShouldCloseStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(true);
			mock.Setup(s => s.Close()).Verifiable();

			var reader = new TsvReader(mock.Object);
			reader.Close();

			mock.Verify();
		}

		[Test]
		public void TsvReader_ReadLine_MinimalTest()
		{
			// this is a minimal test as currently the code calls external class which should be tested
			byte[] bytes = Encoding.ASCII.GetBytes("Hello\tWorld");

			var ms = new MemoryStream();
			ms.Write(bytes, 0, bytes.Length);
			ms.Seek(0, SeekOrigin.Begin);

			var reader = new TsvReader(ms);
			IEnumerable<string> tsv = reader.ReadLine();

			Assert.AreEqual(2, tsv.Count());
			Assert.AreEqual("Hello", tsv.ElementAt(0));
			Assert.AreEqual("World", tsv.ElementAt(1));
		}
	}

}
