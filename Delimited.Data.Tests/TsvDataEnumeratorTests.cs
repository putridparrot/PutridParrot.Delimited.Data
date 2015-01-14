using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Delimited.Data.Specializations;
using Microsoft.CSharp.RuntimeBinder;
using Xunit;

namespace Delimited.Data.Tests
{
	[ExcludeFromCodeCoverage]
	public class TsvDataEnumeratorTests
	{
		[Fact]
		public void Deserialize_EnumerateAllItems_ShouldListEachSymbolCorrectly()
		{
			const string content = "Symbol\tHigh\tLow\tOpen\tClose\nMSFT\t37.60\t37.30\t37.35\t37.40\nGOOG\t1190\t1181.38\t1189\t1188";

			string[] symbols = { "MSFT", "GOOG" };

			int idx = 0;
			foreach (dynamic row in TsvEnumerator.Deserialize(Utils.ToStream(content), new DelimitedDeserializeOptions { UseHeadings = true }))
			{
				Assert.Equal(symbols[idx], row.Symbol);
				Console.WriteLine(String.Format("{0} {1} {2}", row.Symbol, row.High, row.Low));
				idx++;
			}
		}

		[Fact]
		public void DeserializeNoOptions_EnumerateAllItems_ShouldListEachSymbolCorrectly()
		{
			const string content = "Symbol\tHigh\tLow\tOpen\tClose\nMSFT\t37.60\t37.30\t37.35\t37.40\nGOOG\t1190\t1181.38\t1189\t1188";

			string[] symbols = { "MSFT", "GOOG" };

			int idx = 0;
			foreach (dynamic row in TsvEnumerator.Deserialize(Utils.ToStream(content)))
			{
				Assert.Equal(symbols[idx], row.Symbol);
				Console.WriteLine(String.Format("{0} {1} {2}", row.Symbol, row.High, row.Low));
				idx++;
			}
		}

		[Fact]
		public void DeserializeUsingString_EnumerateAllItems_ShouldListEachSymbolCorrectly()
		{
			const string content = "Symbol\tHigh\tLow\tOpen\tClose\nMSFT\t37.60\t37.30\t37.35\t37.40\nGOOG\t1190\t1181.38\t1189\t1188";

			string[] symbols = { "MSFT", "GOOG" };

			int idx = 0;
			foreach (dynamic row in TsvEnumerator.Deserialize(content, new DelimitedDeserializeOptions { UseHeadings = true }))
			{
				Assert.Equal(symbols[idx], row.Symbol);
				Console.WriteLine(String.Format("{0} {1} {2}", row.Symbol, row.High, row.Low));
				idx++;
			}
		}

		[Fact]
		public void DeserializeUsingStringNoOptions_EnumerateAllItems_ShouldListEachSymbolCorrectly()
		{
			const string content = "Symbol\tHigh\tLow\tOpen\tClose\nMSFT\t37.60\t37.30\t37.35\t37.40\nGOOG\t1190\t1181.38\t1189\t1188";

			string[] symbols = { "MSFT", "GOOG" };

			int idx = 0;
			foreach (dynamic row in TsvEnumerator.Deserialize(content))
			{
				Assert.Equal(symbols[idx], row.Symbol);
				Console.WriteLine(String.Format("{0} {1} {2}", row.Symbol, row.High, row.Low));
				idx++;
			}
		}

		[Fact]
		public void Deserialize_EnumerateAllItems_ShouldListEachSymbolCorrectly_UsingIndexer()
		{
			const string content = "Symbol\tHigh\tLow\tOpen\tClose\nMSFT\t37.60\t37.30\t37.35\t37.40\nGOOG\t1190\t1181.38\t1189\t1188";

			string[] symbols = { "MSFT", "GOOG" };

			int idx = 0;
			foreach (dynamic row in TsvEnumerator.Deserialize(Utils.ToStream(content), new DelimitedDeserializeOptions { UseHeadings = true }))
			{
				Assert.Equal(symbols[idx], row["Symbol"]);
				Console.WriteLine(String.Format("{0} {1} {2}", row["Symbol"], row["High"], row["Low"]));
				idx++;
			}
		}

		[Fact]
		public void Deserialize_UsingALinqQueryToGetPartialDataAndTypeInferCorrectly()
		{
			const string content = "Symbol\tHigh\tLow\tOpen\tClose\nMSFT\t37.60\t37.30\t37.35\t37.40\nGOOG\t1190\t1181.38\t1189\t1188";

			var ds = TsvEnumerator.Deserialize(Utils.ToStream(content), new DelimitedDeserializeOptions { UseHeadings = true });

			var query = from dynamic r in ds where r.Open > 100 select r;
			foreach (var row in query)
			{
				Assert.Equal("GOOG", row.Symbol);
				Console.WriteLine(String.Format("{0} {1} {2}", row.Symbol, row.High, row.Low));
			}

		}

		[Fact]
		public void Deserialize_UsingALinqQueryToGetPartialDataAndTypeInferCorrectly_NotHeadersSuppliedOrExpected()
		{
			const string content = "MSFT\t37.60\t37.30\t37.35\t37.40\nGOOG\t1190\t1181.38\t1189\t1188";

			var ds = TsvEnumerator.Deserialize(Utils.ToStream(content), new DelimitedDeserializeOptions { UseHeadings = false });

			var query = from dynamic r in ds where r.Column3 > 100 select r;
			foreach (var row in query)
			{
				Console.WriteLine(row[0]);
				Assert.Equal("GOOG", row.Column1);
				Console.WriteLine(String.Format("{0} {1} {2}", row.Column1, row.Column2, row.Column3));
			}
		}

		[Fact]
		public void Deserialize_UsingALinqQueryToGetPartialDataAndTypeInferCorrectly_NotHeadersSuppliedOrExpected_UsingIndexing()
		{
			const string content = "MSFT\t37.60\t37.30\t37.35\t37.40\nGOOG\t1190\t1181.38\t1189\t1188";

			var ds = TsvEnumerator.Deserialize(Utils.ToStream(content), new DelimitedDeserializeOptions { UseHeadings = false });


			var query = from dynamic r in ds where r[2] > 100 select r;
			foreach (var row in query)
			{
				Assert.Equal("GOOG", row[0]);
				Console.WriteLine(String.Format("{0} {1} {2}", row[0], row[1], row[2]));
			}
		}

		[Fact]
		public void Deserialize_TryToAccessMissingProperty()
		{
			const string content = "Symbol\tHigh\tLow\tOpen\tClose\nMSFT\t37.60\t37.30\t37.35\t37.40\nGOOG\t1190\t1181.38\t1189\t1188";

			Assert.Throws<RuntimeBinderException>(() =>
			{
				foreach (dynamic row in TsvEnumerator.Deserialize(Utils.ToStream(content), new DelimitedDeserializeOptions { UseHeadings = true }))
				{
					Console.WriteLine(row.Artist);
				}
			});
		}

		[Fact]
		public void Deserialize_TryToAccessMissingIndex()
		{
			const string content = "Symbol\tHigh\tLow\tOpen\tClose\nMSFT\t37.60\t37.30\t37.35\t37.40\nGOOG\t1190\t1181.38\t1189\t1188";

			Assert.Throws<RuntimeBinderException>(() =>
			{
				foreach (dynamic row in TsvEnumerator.Deserialize(Utils.ToStream(content), new DelimitedDeserializeOptions { UseHeadings = true }))
				{
					Console.WriteLine(row["Artist"]);
				}
			});
		}
	}

}
