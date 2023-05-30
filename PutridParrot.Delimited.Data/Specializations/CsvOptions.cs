using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace PutridParrot.Delimited.Data.Specializations
{
	/// <summary>
	/// The "standard" Csv options
	/// </summary>
	public class CsvOptions : DelimitedOptions
	{
		[DebuggerStepThrough]
		public CsvOptions() :
			base(',')
		{
		}
	}
}
