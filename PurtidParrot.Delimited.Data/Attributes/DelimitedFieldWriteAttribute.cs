using System;
using System.Diagnostics.CodeAnalysis;

namespace PutridParrot.Delimited.Data.Attributes
{
	/// <summary>
	/// Marks a property or field as a writeable data field.
	/// </summary>
	[ExcludeFromCodeCoverage,
	SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"),
	AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class DelimitedFieldWriteAttribute : DelimitedFieldAttribute
	{
		public DelimitedFieldWriteAttribute()
		{
		}

		public DelimitedFieldWriteAttribute(string heading) :
			base(heading)
		{
		}

		public DelimitedFieldWriteAttribute(int column) :
			base(column)
		{
		}
	}

}
