using System;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using PutridParrot.Delimited.Data.Utils;

namespace PutridParrot.Delimited.Data
{
	/// <summary>
	/// Allows a row of delimited data to be accessed using column heading names
	/// </summary>
	/// <example>
	/// via the column heading
	/// 
	/// i.e. dynamic r = r.MyColumnHeading;
	/// 
	/// or via an indexer
	/// 
	/// i.e. dynamic r = r["MyColumnHeading"];
	/// 
	/// or via the column index
	/// 
	/// i.e. dynamic r = r[0]
	/// 
	/// where columns are zero indexed
	/// 
	/// or if the data doesn't have a header via named properties such as
	/// 
	/// dynamic r = r.Column1
	/// 
	/// where columns names are indexed from 1
	/// </example>
	public class DelimitedRow : DynamicObject
	{
		private readonly string[] _headings;
		private readonly string?[] _fields;

		[DebuggerStepThrough]
		static DelimitedRow()
		{
			CultureInfo = CultureInfo.CurrentCulture;
		}

		[DebuggerStepThrough]
		internal DelimitedRow(string[] headings, string?[] fields)
		{
			_headings = headings;
			_fields = fields;
		}

		private bool GetField(string header, out string? field)
		{
			field = null;
			for (var i = 0; i < _headings.Length; i++)
			{
				if (_headings[i] == header)
				{
					if (i < _fields.Length)
					{
						field = _fields[i];
						return true;
					}
				}
			}
			return false;
		}

		public static CultureInfo CultureInfo { get; set; }

		public override bool TryGetIndex(GetIndexBinder binder, object[]? indexes, out object? result)
		{
			result = null;

			if (indexes?.Length > 0)
			{
				if (indexes[0].IsNumeric())
				{
					var idx = (int)indexes[0];
					if (idx < _fields.Length)
					{
						result = TypeExtensions.ConvertToInferredType(_fields[idx], CultureInfo);
						return true;
					}
				}
				else if (indexes[0].IsString())
				{
					if (GetField((string)indexes[0], out var field))
					{
						result = TypeExtensions.ConvertToInferredType(field, CultureInfo);
						return true;
					}
				}
			}
			return false;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object? result)
		{
			if (binder == null)
			{
				throw new ArgumentNullException(nameof(binder));
			}

			result = null;

            if (GetField(binder.Name, out var field))
			{
				result = TypeExtensions.ConvertToInferredType(field, CultureInfo);
				return true;
			}
			return false;
		}
	}
}
