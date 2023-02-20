using System.Diagnostics;

namespace PutridParrot.Delimited.Data
{
	/// <summary>
	/// Used to define the delimited options which tells other classes
	/// what to use as the delimiter etc.
	/// </summary>
	public class DelimitedOptions
	{
		/// <summary>
		/// Initializes the delimited options
		/// </summary>
		/// <param name="delimiter"></param>
		/// <param name="qualifier"></param>
		/// <param name="qualifyAll"></param>
		[DebuggerStepThrough]
		public DelimitedOptions(char delimiter, char qualifier = '"', bool qualifyAll = false)
		{
			Delimiter = delimiter;
			Qualifier = qualifier;
			QualifyAll = qualifyAll;
		}

		/// <summary>
		/// Gets/Sets the delimiter character
		/// </summary>
		public char Delimiter { get; set; }
		/// <summary>
		/// Gets/Sets the qualifier character
		/// </summary>
		public char Qualifier { get; set; }
		/// <summary>
		/// Gets/Sets whether to qualify all data
		/// </summary>
		public bool QualifyAll { get; set; }
	} 
}
