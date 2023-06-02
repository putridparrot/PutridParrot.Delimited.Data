using System;

namespace PutridParrot.Delimited.Data.Attributes
{
	/// <summary>
	/// Base class for read/writer data field attributes. Declared as abstract to ensure it
	/// cannot be instantiated directly
	/// </summary>
	public abstract class DelimitedFieldAttribute : Attribute
	{
		public int ColumnIndex { get; set; }
		public string? Heading { get; set; }

		protected DelimitedFieldAttribute()
			: this(-1)
		{
		}

		protected DelimitedFieldAttribute(string? heading)
			: this(-1)
		{
			Heading = heading;
		}

		protected DelimitedFieldAttribute(int columnIndex)
		{
			ColumnIndex = columnIndex;
        }
	}
}
