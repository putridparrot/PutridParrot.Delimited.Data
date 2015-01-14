using System;
using System.ComponentModel;

namespace Delimited.Data.Utils
{
	/// <summary>
	/// Allows serialization code to read Y/N and true/false
	/// </summary>
	public class ExtendedBooleanConvertor : BooleanConverter
	{
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			var s = value as string;
			if (s != null)
			{
				string tmp = s.Trim();
				// just going to add ability to understand N == False and Y == True
				if (tmp.Equals("N", StringComparison.CurrentCultureIgnoreCase))
					return false;
				if (tmp.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
					return true;
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}
