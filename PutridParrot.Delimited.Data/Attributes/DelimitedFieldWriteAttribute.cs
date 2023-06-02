using System;
using System.Diagnostics.CodeAnalysis;

namespace PutridParrot.Delimited.Data.Attributes
{
	/// <summary>
	/// Marks a property or field as a writable data field.
	/// </summary>
	[ExcludeFromCodeCoverage,
	AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class DelimitedFieldWriteAttribute : DelimitedFieldAttribute
	{
		public DelimitedFieldWriteAttribute()
		{
		}

		public DelimitedFieldWriteAttribute(string? heading) :
			base(heading)
		{
		}

		public DelimitedFieldWriteAttribute(int column) :
			base(column)
		{
		}
	}
}
