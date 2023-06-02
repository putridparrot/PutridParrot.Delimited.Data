using System;
using System.Collections.Generic;
using System.Globalization;

namespace PutridParrot.Delimited.Data.Utils
{
	// needs to move to a more generic assembly
	public static class TypeExtensions
	{
		private static readonly HashSet<Type> NumericTypes = new HashSet<Type>
		{
			typeof(byte), typeof(sbyte), typeof(short),
			typeof(ushort), typeof(int), typeof(uint),
			typeof(decimal), typeof(double), typeof(float)
		};

		public static bool IsNumeric(this object? o)
		{
			return o != null && IsNumericType(o.GetType());
		}

		public static bool IsNumericType(this Type type)
		{
			return NumericTypes.Contains(type);
		}

		public static bool IsBoolean(this object? o)
		{
			return o != null && IsBooleanType(o.GetType());
		}

		public static bool IsBooleanType(this Type type)
		{
			return Type.GetTypeCode(type) == TypeCode.Boolean;
		}

		public static bool IsString(this object? o)
		{
			return o != null && IsStringType(o.GetType());
		}

		public static bool IsStringType(this Type type)
		{
			return Type.GetTypeCode(type) == TypeCode.String;
		}

		public static bool IsDateTimeType(this Type type)
		{
			return Type.GetTypeCode(type) == TypeCode.DateTime;
		}

		// There must be a better way to do this
		public static object? ConvertToInferredType(string? field, CultureInfo cultureInfo)
		{
			if (bool.TryParse(field, out var b))
			{
				return b;
			}

			if (Int32.TryParse(field, out var i32))
			{
				return i32;
			}

			if (Int64.TryParse(field, out var i64))
			{
				return i64;
			}

			if (float.TryParse(field, out var f))
			{
				return f;
			}

			if (double.TryParse(field, out var d))
			{
				return d;
			}

			if (DateTime.TryParseExact(field, cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo, DateTimeStyles.None, out var shortDateTime))
			{
				return shortDateTime;
			}
			if (DateTime.TryParseExact(field, cultureInfo.DateTimeFormat.LongDatePattern, cultureInfo, DateTimeStyles.None, out var longDateTime))
			{
				return longDateTime;
			}

			return field;
		}
    }
}
