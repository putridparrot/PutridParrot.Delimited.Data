using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Delimited.Data.Attributes;

namespace Delimited.Data
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
