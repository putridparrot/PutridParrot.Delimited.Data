using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Delimited.Data.Specializations
{
	/// <summary>
	/// The standard Tsv options
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tsv", Justification = "Valid acronym for Tab Separated Value")]
	public class TsvOptions : DelimitedOptions
	{
		[DebuggerStepThrough]
		public TsvOptions() :
			base('\t')
		{
		}
	}
}
