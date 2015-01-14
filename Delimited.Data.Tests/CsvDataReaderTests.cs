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
	public class CsvDataReaderTests
	{
		[Fact]
		public void CsvReader_Ctor_NonReadableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(false);

			Assert.Throws<DelimitedStreamReaderException>(() => { CsvReader reader = new CsvReader(mock.Object); });
		}

		[Fact]
		public void CsvReader_Ctor2_NonReadableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(false);

			Assert.Throws<DelimitedStreamReaderException>(() => { CsvReader reader = new CsvReader(mock.Object, Encoding.ASCII); });
		}

		[Fact]
		public void CsvReader_CloseImplicitlyCalledViaDispose()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(true);
			mock.Setup(s => s.Close()).Verifiable();

			var reader = new CsvReader(mock.Object);
			reader.Dispose();

			mock.Verify();
		}

		[Fact]
		public void CsvReader_Close_ShouldCloseStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanRead).Returns(true);
			mock.Setup(s => s.Close()).Verifiable();

			var reader = new CsvReader(mock.Object);
			reader.Close();

			mock.Verify();
		}

		[Fact]
		public void CsvReader_ReadLine_MinimalTest()
		{
			var reader = new CsvReader(Utils.ToStream("Hello, World"));

			IList<string> csv = reader.ReadLine();

			Assert.Equal(2, csv.Count);
			Assert.Equal("Hello", csv[0]);
			Assert.Equal("World", csv[1]);
		}

		[Fact]
		public void CsvReader_ReadOneItemWithQuotes()
		{
			var reader = new CsvReader(Utils.ToStream("\"Hello, World\""));
			IList<string> csv = reader.ReadLine();

			Assert.Equal(1, csv.Count);
			Assert.Equal("Hello, World", csv[0]);
		}

		[Fact]
		public void CsvReader_CheckOptionsSetterGetter()
		{
			var options = new DelimitedOptions('.');

			var reader = new CsvReader(Utils.ToStream("\"Hello, World\""))
			{
				Options = options
			};

			Assert.Equal(options, reader.Options);
		}
	}
}
