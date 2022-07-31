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
