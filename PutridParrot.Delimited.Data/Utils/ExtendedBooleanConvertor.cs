using System;
using System.ComponentModel;
using System.Globalization;

namespace PutridParrot.Delimited.Data.Utils
{
	/// <summary>
	/// Allows serialization code to read Y/N and true/false
	/// </summary>
	public class ExtendedBooleanConvertor : BooleanConverter
	{
        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string s)
            {
                var tmp = s.Trim();
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
