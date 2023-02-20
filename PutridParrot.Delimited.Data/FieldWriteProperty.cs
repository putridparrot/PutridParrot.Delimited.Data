using System;
using System.Diagnostics;
using System.Reflection;
using PutridParrot.Delimited.Data.Attributes;

namespace PutridParrot.Delimited.Data
{
	[Serializable]
	public class FieldWriteProperty
	{
		[DebuggerStepThrough]
		public FieldWriteProperty(PropertyInfo key, DelimitedFieldWriteAttribute value)
		{
			Key = key;
			Value = value;
		}

		public PropertyInfo Key { get; set; }
		public DelimitedFieldWriteAttribute Value { get; set; }
	}
}
