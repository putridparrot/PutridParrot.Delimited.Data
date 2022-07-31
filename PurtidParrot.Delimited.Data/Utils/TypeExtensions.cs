using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Delimited.Data.Utils
{
	// needs to move to a more generic assembly
	public static class TypeExtensions
	{
		private readonly static HashSet<Type> numericTypes = new HashSet<Type>
		{
			typeof(byte), typeof(sbyte), typeof(short),
			typeof(ushort), typeof(int), typeof(uint),
			typeof(decimal), typeof(double), typeof(float)
		};

		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "o")]
		public static bool IsNumeric(this object o)
		{
			return o != null && IsNumericType(o.GetType());
		}

		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "o")]
		public static bool IsNumericType(this Type type)
		{
			return numericTypes.Contains(type);
		}

		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "o")]
		public static bool IsBoolean(this object o)
		{
			return o != null && IsBooleanType(o.GetType());
		}

		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "o")]
		public static bool IsBooleanType(this Type type)
		{
			return Type.GetTypeCode(type) == TypeCode.Boolean;
		}

		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "o")]
		public static bool IsString(this object o)
		{
			return o != null && IsStringType(o.GetType());
		}

		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "o")]
		public static bool IsStringType(this Type type)
		{
			return Type.GetTypeCode(type) == TypeCode.String;
		}

		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "o")]
		public static bool IsDateTimeType(this Type type)
		{
			return Type.GetTypeCode(type) == TypeCode.DateTime;
		}

		// There must be a better way to do this
		public static object ConvertToInferredType(string field, CultureInfo cultureInfo)
		{
			bool b;
			if (bool.TryParse(field, out b))
			{
				return b;
			}

			Int32 i32;
			if (Int32.TryParse(field, out i32))
			{
				return i32;
			}

			Int64 i64;
			if (Int64.TryParse(field, out i64))
			{
				return i64;
			}

			float f;
			if (float.TryParse(field, out f))
			{
				return f;
			}

			double d;
			if (double.TryParse(field, out d))
			{
				return d;
			}

			DateTime dt;
			if (DateTime.TryParseExact(field, cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo, DateTimeStyles.None, out dt))
			{
				return dt;
			}
			if (DateTime.TryParseExact(field, cultureInfo.DateTimeFormat.LongDatePattern, cultureInfo, DateTimeStyles.None, out dt))
			{
				return dt;
			}

			return field;
		}

	}
}
