﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using PutridParrot.Delimited.Data.Exceptions;
using PutridParrot.Delimited.Data.Specializations;
using Moq;
using NUnit.Framework;

namespace Tests.PutridParrot.Delimited.Data
{
	[ExcludeFromCodeCoverage]
	public class TsvWriterTests
	{
		[Test]
		public void TsvWriter_Ctor_NonWritableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanWrite).Returns(false);

			Assert.Throws<DelimitedStreamWriterException>(() => { TsvWriter writer = new TsvWriter(mock.Object); });
		}

		[Test]
		public void TsvWriter_Ctor2_NonWriteableStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanWrite).Returns(false);

			Assert.Throws<DelimitedStreamWriterException>(() => { TsvWriter writer = new TsvWriter(mock.Object, Encoding.ASCII); });
		}

		[Test]
		public void TsvWriter_CloseImplicitlyCalledViaDispose()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanWrite).Returns(true);
			mock.Setup(s => s.Close()).Verifiable();

			var writer = new TsvWriter((mock.Object));
			writer.Dispose();

			mock.Verify();
		}

		[Test]
		public void TsvWriter_Close_ShouldCloseStream()
		{
			var mock = new Mock<Stream>();
			mock.Setup(s => s.CanWrite).Returns(true);
			mock.Setup(s => s.Close()).Verifiable();

			var writer = new TsvWriter(mock.Object);
			writer.Close();

			mock.Verify();
		}

		[Test]
		public void TsvWriter_WriteLine()
		{
			var ms = new MemoryStream();

			var writer = new TsvWriter(ms) {Options = new TsvOptions()};
			writer.WriteLine(new[] { "Hello", "World" });

			writer.Flush();
			ms.Position = 0;

			var reader = new StreamReader(ms);
			var result = reader.ReadToEnd();

			Assert.AreEqual($"Hello\tWorld{Environment.NewLine}", result);
		}

		[Test]
		public void TsvWriter_EnsureOptionsReturnsCorrectValue()
		{
			var ms = new MemoryStream();
			var options = new TsvOptions();

			var writer = new TsvWriter(ms) {Options = options};

			Assert.AreEqual(options, writer.Options);
		}
	}

}
