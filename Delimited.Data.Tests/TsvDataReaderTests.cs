using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Delimited.Data.Exceptions;
using Delimited.Data.Specializations;
using Moq;
using Xunit;

namespace Delimited.Data.Tests
{
	[ExcludeFromCodeCoverage]
	public class TsvDataReaderTests
	{
		[Fact]
		public void TsvReader_Ctor_NonReadableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(false);

			Assert.Throws<DelimitedStreamReaderException>(() => { TsvReader reader = new TsvReader(mock.Object); });
		}

		[Fact]
		public void TsvReader_Ctor2_NonReadableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(false);

			Assert.Throws<DelimitedStreamReaderException>(() => { TsvReader reader = new TsvReader(mock.Object, Encoding.ASCII); });
		}

		[Fact]
		public void TsvReader_CloseImplicitlyCalledViaDispose()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(true);
			mock.Setup(s => s.Close()).Verifiable();

			var reader = new TsvReader(mock.Object);
			reader.Dispose();

			mock.Verify();
		}

		[Fact]
		public void TsvReader_Close_ShouldCloseStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(true);
			mock.Setup(s => s.Close()).Verifiable();

			var reader = new TsvReader(mock.Object);
			reader.Close();

			mock.Verify();
		}

		[Fact]
		public void TsvReader_ReadLine_MinimalTest()
		{
			// this is a minimal test as currently the code calls external class which should be tested
			byte[] bytes = Encoding.ASCII.GetBytes("Hello\tWorld");

			var ms = new MemoryStream();
			ms.Write(bytes, 0, bytes.Length);
			ms.Seek(0, SeekOrigin.Begin);

			var reader = new TsvReader(ms);
			IEnumerable<string> tsv = reader.ReadLine();

			Assert.Equal(2, tsv.Count());
			Assert.Equal("Hello", tsv.ElementAt(0));
			Assert.Equal("World", tsv.ElementAt(1));
		}
	}

}
