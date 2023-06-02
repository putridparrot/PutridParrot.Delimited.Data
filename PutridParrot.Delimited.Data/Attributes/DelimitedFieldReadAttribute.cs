using System;
using System.Diagnostics.CodeAnalysis;

namespace PutridParrot.Delimited.Data.Attributes
{
	/// <summary>
	/// Marks a property or field as a readable data field.
	/// </summary>
	[ExcludeFromCodeCoverage,
	AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class DelimitedFieldReadAttribute : DelimitedFieldAttribute
	{
		public DelimitedFieldReadAttribute()
		{
		}

		public DelimitedFieldReadAttribute(string? heading) :
			this(heading, false)
		{
		}

		public DelimitedFieldReadAttribute(string? heading, bool required) :
			base(heading)
		{
			Required = required;
		}

		public DelimitedFieldReadAttribute(int columnIndex) :
			this(columnIndex, false)
		{
		}

		public DelimitedFieldReadAttribute(int columnIndex, bool required) :
			base(columnIndex)
		{
			Required = required;
		}

		public bool Required { get; set; }
		public string[]? AlternateNames { get; set; }
	}
}
