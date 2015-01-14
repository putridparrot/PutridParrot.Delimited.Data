using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Delimited.Data.Attributes;
using Delimited.Data.Exceptions;
using Delimited.Data.Specializations;
using Xunit;

namespace Delimited.Data.Tests
{
	[ExcludeFromCodeCoverage]
	public class CsvSerializerTests
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

		class Person
		{
			[DelimitedFieldRead("Age", AlternateNames = new[] { "A" })]
			[DelimitedFieldWrite("Age", ColumnIndex = 2)]
			public int Age { get; set; }
			[DelimitedFieldRead("Name", AlternateNames = new[] { "N" }, Required = true)]
			[DelimitedFieldWrite("Name", ColumnIndex = 1)]
			public string Name { get; set; }
			[DelimitedFieldRead("Updated", AlternateNames = new[] { "U" })]
			[DelimitedFieldWrite("Updated", ColumnIndex = 0)]
			public DateTime Updated { get; set; }
			[DelimitedFieldRead("Employed", AlternateNames = new[] { "E" })]
			[DelimitedFieldWrite("Employed", ColumnIndex = 3)]
			public bool Employed { get; set; }
			[DelimitedFieldRead("Married", AlternateNames = new[] { "M" })]
			[DelimitedFieldWrite("Married", ColumnIndex = 4)]
			public bool Married { get; set; }
		}

		private IList<Person> Get()
		{
			var items = new List<Person>
			{
				new Person {Name = "Scooby Doo", Age = 12, Updated = new DateTime(2013, 11, 7, 11, 0, 0)},
				new Person {Name = "Shaggy", Age = 24, Updated = new DateTime(2013, 11, 6, 10, 0, 0)},
				new Person {Name = "Scrappy", Age = 5, Updated = new DateTime(2013, 10, 1, 9, 9, 9)}
			};
			return items;
		}

		[Fact]
		public void Serialize()
		{
			var ms = new MyStream();

			IList<Person> items = Get();

			CsvSerializer<Person>.Serialize(ms, items, new DelimitedSerializeOptions { IncludeHeadings = true });

			ms.Position = 0;

			var reader = new StreamReader(ms);
			string output = reader.ReadToEnd();

			string[] split = output.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			ms.ForceClose();

			Assert.Equal(4, split.Length);
			Assert.Equal("Updated,Name,Age,Employed,Married", split[0]);
		}

		[Fact]
		public void Deserialize()
		{
			Stream ms = Utils.ToStream("Updated,Name,Age\r\n20/11/2003,Road Runner,11");

			IList<Person> items = new List<Person>(CsvSerializer<Person>.Deserialize(ms, new DelimitedDeserializeOptions { UseHeadings = true }));

			Assert.Equal(1, items.Count);
			Assert.Equal(new DateTime(2003, 11, 20), items[0].Updated);
			Assert.Equal("Road Runner", items[0].Name);
			Assert.Equal(11, items[0].Age);
		}

		[Fact]
		public void Deserialize_WithMissingHeader()
		{
			Stream ms = Utils.ToStream("20/11/2003,Road Runner,11");

			Assert.Throws<DelimitedSerializationException>(() =>
			{
				foreach (var line in CsvSerializer<Person>.Deserialize(ms,
					new DelimitedDeserializeOptions { UseHeadings = true }))
				{
					// this should exception
				}
			});
		}

		[Fact]
		public void Deserialize_UsingAlternateNames()
		{
			Stream ms = Utils.ToStream("U,N,A\r\n20/11/2003,Road Runner,11");

			IList<Person> items = new List<Person>(CsvSerializer<Person>.Deserialize(ms, new DelimitedDeserializeOptions { UseHeadings = true }));

			Assert.Equal(1, items.Count);
			Assert.Equal(new DateTime(2003, 11, 20), items[0].Updated);
			Assert.Equal("Road Runner", items[0].Name);
			Assert.Equal(11, items[0].Age);
		}

		[Fact]
		public void Deserialize_UsingBooleanYN()
		{
			Stream ms = Utils.ToStream("U,N,A,E,M\r\n20/11/2003,Road Runner,11,Y,N");

			IList<Person> items = new List<Person>(CsvSerializer<Person>.Deserialize(ms, new DelimitedDeserializeOptions { UseHeadings = true }));

			Assert.Equal(1, items.Count);
			Assert.Equal(new DateTime(2003, 11, 20), items[0].Updated);
			Assert.Equal("Road Runner", items[0].Name);
			Assert.Equal(11, items[0].Age);
			Assert.Equal(true, items[0].Employed);
		}

		[Fact]
		public void Deserialize_UsingBoolean()
		{
			Stream ms = Utils.ToStream("U,N,A,E,M\r\n20/11/2003,Road Runner,11,true,false");

			IList<Person> items = new List<Person>(CsvSerializer<Person>.Deserialize(ms, new DelimitedDeserializeOptions { UseHeadings = true }));

			Assert.Equal(1, items.Count);
			Assert.Equal(new DateTime(2003, 11, 20), items[0].Updated);
			Assert.Equal("Road Runner", items[0].Name);
			Assert.Equal(11, items[0].Age);
			Assert.Equal(true, items[0].Employed);
		}

		[Fact]
		public void Deserialize_WithEmptyRows_DefaultIgnoreEmptyRows()
		{
			Stream ms = Utils.ToStream("Updated,Name,Age\r\n,,\r\n,,\r\n20/11/2003,Road Runner,11\r\n,,\r\n");

			IList<Person> items = new List<Person>(CsvSerializer<Person>.Deserialize(ms, new DelimitedDeserializeOptions { UseHeadings = true }));

			Assert.Equal(1, items.Count);
			Assert.Equal(new DateTime(2003, 11, 20), items[0].Updated);
			Assert.Equal("Road Runner", items[0].Name);
			Assert.Equal(11, items[0].Age);
		}

		[Fact]
		public void Deserialize_WithEmptyRows_IgnoreEmptyRowsSetToFalse()
		{
			Stream ms = Utils.ToStream("Updated,Name,Age\r\n,,\r\n,,\r\n20/11/2003,Road Runner,11\r\n,,\r\n");

			IList<Person> items = new List<Person>(CsvSerializer<Person>.Deserialize(ms, new DelimitedDeserializeOptions { UseHeadings = true, IgnoreEmptyRows = false }));

			Assert.Equal(4, items.Count);

			Assert.Equal(DateTime.MinValue, items[0].Updated);
			Assert.Equal("", items[0].Name);
			Assert.Equal(0, items[0].Age);

			Assert.Equal(DateTime.MinValue, items[1].Updated);
			Assert.Equal("", items[1].Name);
			Assert.Equal(0, items[1].Age);

			Assert.Equal(new DateTime(2003, 11, 20), items[2].Updated);
			Assert.Equal("Road Runner", items[2].Name);
			Assert.Equal(11, items[2].Age);

			Assert.Equal(DateTime.MinValue, items[3].Updated);
			Assert.Equal("", items[3].Name);
			Assert.Equal(0, items[3].Age);
		}

		[Fact]
		public void Deserialize_AlternateDeserializeUsingStream_WithoutOptions()
		{
			Stream ms = Utils.ToStream("Updated,Name,Age\r\n20/11/2003,Road Runner,11\r\n");

			IList<Person> items = new List<Person>(CsvSerializer<Person>.Deserialize(ms));

			Assert.Equal(1, items.Count);

			Assert.Equal(new DateTime(2003, 11, 20), items[0].Updated);
			Assert.Equal("Road Runner", items[0].Name);
			Assert.Equal(11, items[0].Age);
		}

		[Fact]
		public void Deserialize_AlternateDeserializeUsingString_WithoutOptions()
		{
			const string ms = "Updated,Name,Age\r\n20/11/2003,Road Runner,11\r\n";

			IList<Person> items = new List<Person>(CsvSerializer<Person>.Deserialize(ms));

			Assert.Equal(1, items.Count);

			Assert.Equal(new DateTime(2003, 11, 20), items[0].Updated);
			Assert.Equal("Road Runner", items[0].Name);
			Assert.Equal(11, items[0].Age);
		}
	}
}
