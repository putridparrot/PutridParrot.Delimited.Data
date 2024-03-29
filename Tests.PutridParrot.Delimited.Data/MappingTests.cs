﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using PutridParrot.Delimited.Data;
using PutridParrot.Delimited.Data.Exceptions;
using PutridParrot.Delimited.Data.Specializations;

namespace Tests.PutridParrot.Delimited.Data
{
	[ExcludeFromCodeCoverage]
	public class MappingTests
	{
		class Person
		{
			public int Age { get; set; }
			public string Name { get; set; }
			public DateTime Updated { get; set; }
			public bool Employed { get; set; }
			public bool Married { get; set; }
		}

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

		//private IList<Person> Get()
		//{
		//	List<Person> items = new List<Person>();
		//	items.Add(new Person { Name = "Scooby Doo", Age = 12, Updated = new DateTime(2013, 11, 7, 11, 0, 0) });
		//	items.Add(new Person { Name = "Shaggy", Age = 24, Updated = new DateTime(2013, 11, 6, 10, 0, 0) });
		//	items.Add(new Person { Name = "Scrappy", Age = 5, Updated = new DateTime(2013, 10, 1, 9, 9, 9) });
		//	return items;
		//}

        [SetUp]
        public void SetUp()
        {
            var culture = new System.Globalization.CultureInfo("en-GB");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

		[Test]
		public void GenerateReadMappings_FromValidMappingsStartAndEndElements()
		{
			var ms = Utils.ToStream(
							 "<DelimitedSerializerMappings>" +
							 "   <Mapping Heading=\"Heading Updated\" Property=\"Updated\"></Mapping>" +
							 "   <Mapping Heading=\"Heading Name\" Property=\"Name\"></Mapping>" +
							 "   <Mapping Heading=\"Heading Age\" Property=\"Age\"></Mapping>" +
							 "</DelimitedSerializerMappings>");

			var mappings = DelimitedSerializer<Person>.GenerateReadMappings(ms);
			Assert.AreEqual(3, mappings.Count);

			Assert.NotNull(mappings.FirstOrDefault(m => m.Key.Name == "Updated" && m.Value.Heading == "Heading Updated"));
			Assert.NotNull(mappings.FirstOrDefault(m => m.Key.Name == "Name" && m.Value.Heading == "Heading Name"));
			Assert.NotNull(mappings.FirstOrDefault(m => m.Key.Name == "Age" && m.Value.Heading == "Heading Age"));
		}

		[Test]
		public void GenerateWriteMappings_FromValidMappingsStartAndEndElements()
		{
			var ms = Utils.ToStream(
								"<DelimitedSerializerMappings>" +
								"   <Mapping Heading=\"Heading Updated\" Property=\"Updated\"></Mapping>" +
								"   <Mapping Heading=\"Heading Name\" Property=\"Name\"></Mapping>" +
								"   <Mapping Heading=\"Heading Age\" Property=\"Age\"></Mapping>" +
								"</DelimitedSerializerMappings>");

			var mappings = DelimitedSerializer<Person>.GenerateWriteMappings(ms);
			Assert.AreEqual(3, mappings.Count);

			Assert.NotNull(mappings.FirstOrDefault(m => m.Key.Name == "Updated" && m.Value.Heading == "Heading Updated"));
			Assert.NotNull(mappings.FirstOrDefault(m => m.Key.Name == "Name" && m.Value.Heading == "Heading Name"));
			Assert.NotNull(mappings.FirstOrDefault(m => m.Key.Name == "Age" && m.Value.Heading == "Heading Age"));
		}

		[Test]
		public void GenerateReadMappings_FromValidMappingsSingleElements()
		{
			var ms = Utils.ToStream(
								"<DelimitedSerializerMappings>" +
								"   <Mapping Heading=\"Heading Updated\" Property=\"Updated\"/>" +
								"   <Mapping Heading=\"Heading Name\" Property=\"Name\"/>" +
								"   <Mapping Heading=\"Heading Age\" Property=\"Age\"/>" +
								"</DelimitedSerializerMappings>");

			var mappings = DelimitedSerializer<Person>.GenerateReadMappings(ms);
			Assert.AreEqual(3, mappings.Count);

			Assert.NotNull(mappings.FirstOrDefault(m => m.Key.Name == "Updated" && m.Value.Heading == "Heading Updated"));
			Assert.NotNull(mappings.FirstOrDefault(m => m.Key.Name == "Name" && m.Value.Heading == "Heading Name"));
			Assert.NotNull(mappings.FirstOrDefault(m => m.Key.Name == "Age" && m.Value.Heading == "Heading Age"));
		}

		[Test]
		public void GenerateWriteMappings_FromValidMappingsSingleElements()
		{
			var ms = Utils.ToStream(
								"<DelimitedSerializerMappings>" +
								"   <Mapping Heading=\"Heading Updated\" Property=\"Updated\"/>" +
								"   <Mapping Heading=\"Heading Name\" Property=\"Name\"/>" +
								"   <Mapping Heading=\"Heading Age\" Property=\"Age\"/>" +
								"</DelimitedSerializerMappings>");

			var mappings = DelimitedSerializer<Person>.GenerateWriteMappings(ms);
			Assert.AreEqual(3, mappings.Count);

			Assert.NotNull(mappings.FirstOrDefault(m => m.Key.Name == "Updated" && m.Value.Heading == "Heading Updated"));
			Assert.NotNull(mappings.FirstOrDefault(m => m.Key.Name == "Name" && m.Value.Heading == "Heading Name"));
			Assert.NotNull(mappings.FirstOrDefault(m => m.Key.Name == "Age" && m.Value.Heading == "Heading Age"));
		}

		[Test]
		public void GenerateReadMappings_AndDeserialize()
		{
			var ms = Utils.ToStream(
								"<DelimitedSerializerMappings>" +
								"   <Mapping Heading=\"Heading Updated\" Property=\"Updated\"/>" +
								"   <Mapping Heading=\"Heading Name\" Property=\"Name\"/>" +
								"   <Mapping Heading=\"Heading Age\" Property=\"Age\"/>" +
								"</DelimitedSerializerMappings>");

			var ds = Utils.ToStream("Heading Updated,Heading Name,Heading Age\r\n20/11/2003,Road Runner,11");

			var items = new List<Person>(DelimitedSerializer<Person>.Deserialize(new DelimitedSeparatedReader(new CsvOptions()), ds, new DelimitedDeserializeOptions { UseHeadings = true, Mappings = DelimitedSerializer<Person>.GenerateReadMappings(ms) }));

			Assert.AreEqual(1, items.Count);
			Assert.AreEqual(new DateTime(2003, 11, 20), items[0].Updated);
			Assert.AreEqual("Road Runner", items[0].Name);
			Assert.AreEqual(11, items[0].Age);
		}

		[Test]
		public void GenerateReadMappings_UsingIndicies_AndDeserialize()
		{
			var ms = Utils.ToStream(
								"<DelimitedSerializerMappings>" +
								"   <Mapping ColumnIndex=\"0\" Property=\"Updated\"/>" +
								"   <Mapping ColumnIndex=\"1\" Property=\"Name\"/>" +
								"   <Mapping ColumnIndex=\"2\" Property=\"Age\"/>" +
								"</DelimitedSerializerMappings>");

			var ds = Utils.ToStream("20/11/2003,Road Runner,11");

			var items = new List<Person>(DelimitedSerializer<Person>.Deserialize(new DelimitedSeparatedReader(new CsvOptions()), ds, new DelimitedDeserializeOptions { Mappings = DelimitedSerializer<Person>.GenerateReadMappings(ms) }));

			Assert.AreEqual(1, items.Count);
			Assert.AreEqual(new DateTime(2003, 11, 20), items[0].Updated);
			Assert.AreEqual("Road Runner", items[0].Name);
			Assert.AreEqual(11, items[0].Age);
		}

		[Test]
		public void GenerateReadMappings_UsingIndiciesIgnoreOneRow_AndDeserialize()
		{
			var ms = Utils.ToStream(
								"<DelimitedSerializerMappings>" +
								"   <Mapping Heading=\"Heading Updated\" Property=\"Updated\"/>" +
								"   <Mapping Heading=\"Heading Name\" Property=\"Name\"/>" +
								"   <Mapping Heading=\"Heading Age\" Property=\"Age\"/>" +
								"</DelimitedSerializerMappings>");

			// assume an empty first line to the data, we want to ignore this
			var ds = Utils.ToStream("\r\nHeading Updated,Heading Name,Heading Age\r\n20/11/2003,Road Runner,11");

			var items = new List<Person>(DelimitedSerializer<Person>.Deserialize(new DelimitedSeparatedReader(new CsvOptions()), ds, new DelimitedDeserializeOptions { UseHeadings = true, IgnoreFirstNRows = 1, Mappings = DelimitedSerializer<Person>.GenerateReadMappings(ms) }));

			Assert.AreEqual(1, items.Count);
			Assert.AreEqual(new DateTime(2003, 11, 20), items[0].Updated);
			Assert.AreEqual("Road Runner", items[0].Name);
			Assert.AreEqual(11, items[0].Age);
		}

		[Test]
		public void GenerateReadMappings_AndDeserialize_WithMissingRequiredField_AndEnforceRequiredFieldsOn()
		{
			var ms = Utils.ToStream(
								"<DelimitedSerializerMappings>" +
								"   <Mapping Heading=\"Heading Updated\" Property=\"Updated\" Required=\"true\"/>" +
								"   <Mapping Heading=\"Heading Name\" Property=\"Name\"/>" +
								"   <Mapping Heading=\"Heading Age\" Property=\"Age\"/>" +
								"</DelimitedSerializerMappings>");

			var ds = Utils.ToStream("Heading Name,Heading Age\r\nRoad Runner,11");

			Assert.Throws<DelimitedSerializationException>(() =>
			{
				foreach (var _ in DelimitedSerializer<Person>.Deserialize(new DelimitedSeparatedReader(new CsvOptions()), ds, new DelimitedDeserializeOptions { UseHeadings = true, EnforceRequiredFields = true, Mappings = DelimitedSerializer<Person>.GenerateReadMappings(ms) }))
				{
					// should exception
				}
			});
		}

		[Test]
		public void GenerateWriteMappings_AndSerialize()
		{
			var dataStream = Utils.ToStream("Heading Updated,Heading Name,Heading Age\r\n20/11/2003,Road Runner,11");

			var mappingStream = Utils.ToStream(
								"<DelimitedSerializerMappings>" +
								"   <Mapping Heading=\"Heading Updated\" Property=\"Updated\"/>" +
								"   <Mapping Heading=\"Heading Name\" Property=\"Name\"/>" +
								"   <Mapping Heading=\"Heading Age\" Property=\"Age\"/>" +
								"</DelimitedSerializerMappings>");

            var readMappings = DelimitedSerializer<Person>.GenerateReadMappings(mappingStream);
			mappingStream.Position = 0;
			var writeMappings = DelimitedSerializer<Person>.GenerateWriteMappings(mappingStream);

			var items = new List<Person>(DelimitedSerializer<Person>.Deserialize(new DelimitedSeparatedReader(new CsvOptions()), dataStream, new DelimitedDeserializeOptions { UseHeadings = true, Mappings = readMappings }));

			var writeStream = new MyStream();
			DelimitedSerializer<Person>.Serialize(new DelimitedSeparatedWriter(new CsvOptions()), writeStream, items, new DelimitedSerializeOptions { IncludeHeadings = true, Mappings = writeMappings });
			writeStream.Position = 0;

			var _ = new List<Person>(DelimitedSerializer<Person>.Deserialize(new DelimitedSeparatedReader(new CsvOptions()), writeStream, new DelimitedDeserializeOptions { UseHeadings = true, Mappings = readMappings }));

			writeStream.ForceClose();

			Assert.AreEqual(1, items.Count);
			Assert.AreEqual(new DateTime(2003, 11, 20), items[0].Updated);
			Assert.AreEqual("Road Runner", items[0].Name);
			Assert.AreEqual(11, items[0].Age);
		}
	}
}
