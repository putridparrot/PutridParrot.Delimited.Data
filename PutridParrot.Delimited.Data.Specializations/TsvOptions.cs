using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace PutridParrot.Delimited.Data.Specializations
{
	/// <summary>
	/// The standard Tsv options
	/// </summary>
	public class TsvOptions : DelimitedOptions
	{
		[DebuggerStepThrough]
		public TsvOptions() :
			base('\t')
		{
		}
	}
}
