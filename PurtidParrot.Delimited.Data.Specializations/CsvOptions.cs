using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Delimited.Data.Specializations
{
	/// <summary>
	/// The "standard" Csv options
	/// </summary>
	[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Csv", Justification = "Valid acronym for Comma Separated Value")]
	public class CsvOptions : DelimitedOptions
	{
		[DebuggerStepThrough]
		public CsvOptions() :
			base(',')
		{
		}
	}
}
