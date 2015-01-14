using System;
using System.Diagnostics.CodeAnalysis;
using Delimited.Data.Utils;
using Xunit;

namespace Delimited.Data.Tests
{
	[ExcludeFromCodeCoverage]
	public class TypeExtensionsTests
	{
		[Fact]
		public void IsBoolean_WithNull_ShouldReturnFalse()
		{
			Assert.False(((object)null).IsBoolean());
		}

		[Fact]
		public void IsBoolean_WithNonBoolean_ShouldReturnFalse()
		{
			Assert.False("Hello".IsBoolean());
		}

		[Fact]
		public void IsBoolean_WithBoolean_ShouldReturnTrue()
		{
			Assert.True(true.IsBoolean());
		}

		[Fact]
		public void IsBooleanType_WithNull_ShouldReturnFalse()
		{
			Assert.False(((Type)null).IsBooleanType());
		}

		[Fact]
		public void IsBooleanType_WithNonBoolean_ShouldReturnFalse()
		{
			Assert.False(typeof(string).IsBooleanType());
		}

		[Fact]
		public void IsBooleanType_WithBoolean_ShouldReturnTrue()
		{
			Assert.True(typeof(bool).IsBooleanType());
		}

		[Fact]
		public void IsNumeric_WithNull_ShouldReturnFalse()
		{
			Assert.False(((object)null).IsNumeric());
		}

		[Fact]
		public void IsNumeric_WithNonNumeric_ShouldReturnFalse()
		{
			Assert.False("Hello".IsNumeric());
		}

		[Fact]
		public void IsNumeric_WithNumeric_ShouldReturnTrue()
		{
			Assert.True(123.IsNumeric());
		}

		[Fact]
		public void IsNumericType_WithNull_ShouldReturnFalse()
		{
			Assert.False(((Type)null).IsNumericType());
		}

		[Fact]
		public void IsNumericType_WithNonNumeric_ShouldReturnFalse()
		{
			Assert.False(typeof(string).IsNumericType());
		}

		[Fact]
		public void IsNumericType_WithNumeric_ShouldReturnTrue()
		{
			Assert.True(typeof(int).IsNumericType());
		}
	}

}
