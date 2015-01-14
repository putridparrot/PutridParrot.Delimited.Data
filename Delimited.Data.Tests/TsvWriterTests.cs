using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Delimited.Data.Exceptions;
using Delimited.Data.Specializations;
using Moq;
using Xunit;

namespace Delimited.Data.Tests
{
	[ExcludeFromCodeCoverage]
	public class TsvWriterTests
	{
		[Fact]
		public void TsvWriter_Ctor_NonWriteableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanWrite).Returns(false);

			Assert.Throws<DelimitedStreamWriterException>(() => { TsvWriter writer = new TsvWriter(mock.Object); });
		}

		[Fact]
		public void TsvWriter_Ctor2_NonWriteableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanWrite).Returns(false);

			Assert.Throws<DelimitedStreamWriterException>(() => { TsvWriter writer = new TsvWriter(mock.Object, Encoding.ASCII); });
		}

		[Fact]
		public void TsvWriter_CloseImplicitlyCalledViaDispose()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanWrite).Returns(true);
			mock.Setup(s => s.Close()).Verifiable();

			var writer = new TsvWriter((mock.Object));
			writer.Dispose();

			mock.Verify();
		}

		[Fact]
		public void TsvWriter_Close_ShouldCloseStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanWrite).Returns(true);
			mock.Setup(s => s.Close()).Verifiable();

			var writer = new TsvWriter(mock.Object);
			writer.Close();

			mock.Verify();
		}

		[Fact]
		public void TsvWriter_WriteLine()
		{
			var ms = new MemoryStream();

			var writer = new TsvWriter(ms) {Options = new TsvOptions()};
			writer.WriteLine(new[] { "Hello", "World" });

			writer.Flush();
			ms.Position = 0;

			var reader = new StreamReader(ms);
			string result = reader.ReadToEnd();

			Assert.Equal("Hello\tWorld\r\n", result);
		}

		[Fact]
		public void TsvWriter_EnsureOptionsReturnsCorrectValue()
		{
			var ms = new MemoryStream();
			var options = new TsvOptions();

			var writer = new TsvWriter(ms) {Options = options};

			Assert.Equal(options, writer.Options);
		}
	}

}
