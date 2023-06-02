using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using NUnit.Framework;
using PutridParrot.Delimited.Data.Specializations;
using PutridParrot.Delimited.Data.Tools;

namespace Tests.PutridParrot.Delimited.Data
{
#if DEBUG // needs more testing before it can get release
	[ExcludeFromCodeCoverage]
	public class DelimitedStreamBuilderTests
	{
		class MyStream : MemoryStream
		{
			// we want to stop the serializer automatically disposing of the stream
			public override void Close()
			{
			}

			public void ForceClose()
			{
				base.Close();
			}
		}

		[Test]
		public void Connect_WithNullInput_NoExceptionsExpectedAndNoOutput()
		{
			var ms = new MyStream();
			var writer = new TsvWriter(ms);

			DelimitedStreamBuilder.New().Write(writer);

			Assert.AreEqual(0, ms.Length);
		}

		[Test]
		public void Connect_WithNullOutput()
		{
			var reader = new CsvReader(Utils.ToStream("Hello, World"));
			Assert.Throws<ArgumentNullException>(() => DelimitedStreamBuilder.New(reader).Write(null));
		}

		[Test]
		public void Connect_TurnCsvIntoTsv()
		{
			var ms = new MyStream();

			var reader = new CsvReader(Utils.ToStream("Hello, World"));
			var writer = new TsvWriter(ms);

            DelimitedStreamBuilder.New(reader).Write(writer);

			ms.Position = 0;

			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			ms.ForceClose();

			Assert.AreEqual($"Hello\tWorld{Environment.NewLine}", result);
		}

		[Test]
		public void Connect_TurnCsvIntoTsvSkipFirstThreeLines()
		{
			var ms = new MyStream();

			var reader = new CsvReader(Utils.ToStream("one\r\ntwo\r\nthree\r\nHello, World"));
			var writer = new TsvWriter(ms);

            DelimitedStreamBuilder.New(reader).Write(writer, 3);

			ms.Position = 0;

			var sr = new StreamReader(ms);
			var result = sr.ReadToEnd();

			ms.ForceClose();

			Assert.AreEqual($"Hello\tWorld{Environment.NewLine}", result);
		}
	}
#endif
}
