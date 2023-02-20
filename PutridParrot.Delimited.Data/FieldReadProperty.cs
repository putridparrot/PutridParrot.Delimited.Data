using System;
using System.Diagnostics;
using System.Reflection;
using PutridParrot.Delimited.Data.Attributes;

namespace PutridParrot.Delimited.Data
{
	[Serializable]
	public class FieldReadProperty
	{
		[DebuggerStepThrough]
		public FieldReadProperty(PropertyInfo key, DelimitedFieldReadAttribute value)
		{
			Key = key;
			Value = value;
		}

		public PropertyInfo Key { get; set; }
		public DelimitedFieldReadAttribute Value { get; set; }
	}
}
