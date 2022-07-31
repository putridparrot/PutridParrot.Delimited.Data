using System;
using System.Diagnostics.CodeAnalysis;
using PutridParrot.Delimited.Data.Utils;
using NUnit.Framework;

namespace PutridParrot.Delimited.Data.Tests
{
	[ExcludeFromCodeCoverage]
	public class TypeExtensionsTests
	{
		[Test]
		public void IsBoolean_WithNull_ShouldReturnFalse()
		{
			Assert.False(((object)null).IsBoolean());
		}

		[Test]
		public void IsBoolean_WithNonBoolean_ShouldReturnFalse()
		{
			Assert.False("Hello".IsBoolean());
		}

		[Test]
		public void IsBoolean_WithBoolean_ShouldReturnTrue()
		{
			Assert.True(true.IsBoolean());
		}

		[Test]
		public void IsBooleanType_WithNull_ShouldReturnFalse()
		{
			Assert.False(((Type)null).IsBooleanType());
		}

		[Test]
		public void IsBooleanType_WithNonBoolean_ShouldReturnFalse()
		{
			Assert.False(typeof(string).IsBooleanType());
		}

		[Test]
		public void IsBooleanType_WithBoolean_ShouldReturnTrue()
		{
			Assert.True(typeof(bool).IsBooleanType());
		}

		[Test]
		public void IsNumeric_WithNull_ShouldReturnFalse()
		{
			Assert.False(((object)null).IsNumeric());
		}

		[Test]
		public void IsNumeric_WithNonNumeric_ShouldReturnFalse()
		{
			Assert.False("Hello".IsNumeric());
		}

		[Test]
		public void IsNumeric_WithNumeric_ShouldReturnTrue()
		{
			Assert.True(123.IsNumeric());
		}

		[Test]
		public void IsNumericType_WithNull_ShouldReturnFalse()
		{
			Assert.False(((Type)null).IsNumericType());
		}

		[Test]
		public void IsNumericType_WithNonNumeric_ShouldReturnFalse()
		{
			Assert.False(typeof(string).IsNumericType());
		}

		[Test]
		public void IsNumericType_WithNumeric_ShouldReturnTrue()
		{
			Assert.True(typeof(int).IsNumericType());
		}
	}

}
