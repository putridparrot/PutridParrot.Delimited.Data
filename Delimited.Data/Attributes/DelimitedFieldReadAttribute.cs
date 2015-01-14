using System;
using System.Diagnostics.CodeAnalysis;

namespace Delimited.Data.Attributes
{
	/// <summary>
	/// Marks a property or field as a readable data field.
	/// </summary>
	[ExcludeFromCodeCoverage,
	SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments"),
	AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class DelimitedFieldReadAttribute : DelimitedFieldAttribute
	{
		public DelimitedFieldReadAttribute()
		{
		}

		public DelimitedFieldReadAttribute(string heading) :
			this(heading, false)
		{
		}

		public DelimitedFieldReadAttribute(string heading, bool required) :
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
		public string[] AlternateNames { get; set; }
	}

}
