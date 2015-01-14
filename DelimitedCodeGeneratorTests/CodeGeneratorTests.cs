using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Delimited.Data;
using Delimited.Data.Tests;
using DelimitedCodeGenerator;
using Xunit;

namespace DelimitedCodeGeneratorTests
{
	[ExcludeFromCodeCoverage]
	public class CodeGeneratorTests
	{
		[Fact]
		public void ParseNullStream_ShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => Parser.Parse((Stream)null, false, ',', '"', false, null));
		}

		[Fact]
		public void ParseNullString_ShouldThrowArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => Parser.Parse((string)null, false, ',', '"', false, null));
		}

		[Fact]
		public void ParseEmptyString_ShouldThrowArgumentNullException()
		{
			var codeBuilder = new CSharpCodeBuilder();
			Assert.Throws<FileNotFoundException>(() => Parser.Parse(String.Empty, false, ',', '"', false, codeBuilder));
		}

		[Fact]
		public void Parse_WithHeadings()
		{
			const string data = "Updated,Name,Age\r\n20/11/2003,Road Runner,11";
			Stream ms = Utils.ToStream(data);

			var codeBuilder = new CSharpCodeBuilder();
			string code = Parser.Parse(ms, true, ',', '"', false, codeBuilder);

			Assert.NotNull(code);

			IEnumerable result = ReflectionUtils.InvokeDeserializer(
					ReflectionUtils.GenerateAssembly(code, new[] { "Delimited.Data.dll" }), data, new DelimitedOptions(','));

			Assert.NotNull(result);

			IEnumerator enumerator = result.GetEnumerator();
			enumerator.MoveNext();
			object current = enumerator.Current;
			PropertyInfo[] properties = current.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			Assert.Equal("Road Runner", properties.FirstOrDefault(p => p.Name == "Name").GetValue(current));
			Assert.Equal(11, properties.FirstOrDefault(p => p.Name == "Age").GetValue(current));
			Assert.Equal("20/11/2003", ((DateTime)properties.FirstOrDefault(p => p.Name == "Updated").GetValue(current)).ToString("dd/MM/yyyy"));
		}

		[Fact]
		public void Parse_WithHeadingsWithSpacesInText()
		{
			const string data = "Updated Date,Name,Age\r\n20/11/2003,Road Runner,11";
			Stream ms = Utils.ToStream(data);

			var codeBuilder = new CSharpCodeBuilder();
			string code = Parser.Parse(ms, true, ',', '"', false, codeBuilder);

			Assert.NotNull(code);

			IEnumerable result = ReflectionUtils.InvokeDeserializer(
					ReflectionUtils.GenerateAssembly(code, new[] { "Delimited.Data.dll" }), data, new DelimitedOptions(','));

			Assert.NotNull(result);

			IEnumerator enumerator = result.GetEnumerator();
			enumerator.MoveNext();
			object current = enumerator.Current;
			PropertyInfo[] properties = current.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			Assert.Equal("Road Runner", properties.FirstOrDefault(p => p.Name == "Name").GetValue(current));
			Assert.Equal(11, properties.FirstOrDefault(p => p.Name == "Age").GetValue(current));
			Assert.Equal("20/11/2003", ((DateTime)properties.FirstOrDefault(p => p.Name == "UpdatedDate").GetValue(current)).ToString("dd/MM/yyyy"));
		}

		[Fact]
		public void Parse_WithHeadingsWithInvalidCharacters()
		{
			const string data = "+Updated Date,\"Name\",Age\r\n20/11/2003,Road Runner,11";
			Stream ms = Utils.ToStream(data);

			var codeBuilder = new CSharpCodeBuilder();
			string code = Parser.Parse(ms, true, ',', '"', false, codeBuilder);

			Assert.NotNull(code);

			IEnumerable result = ReflectionUtils.InvokeDeserializer(
					ReflectionUtils.GenerateAssembly(code, new[] { "Delimited.Data.dll" }), data, new DelimitedOptions(','));

			Assert.NotNull(result);

			IEnumerator enumerator = result.GetEnumerator();
			enumerator.MoveNext();
			object current = enumerator.Current;
			PropertyInfo[] properties = current.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

			Assert.Equal("Road Runner", properties.FirstOrDefault(p => p.Name == "Name").GetValue(current));
			Assert.Equal(11, properties.FirstOrDefault(p => p.Name == "Age").GetValue(current));
			Assert.Equal("20/11/2003", ((DateTime)properties.FirstOrDefault(p => p.Name == "UpdatedDate").GetValue(current)).ToString("dd/MM/yyyy"));
		}
	}

}
