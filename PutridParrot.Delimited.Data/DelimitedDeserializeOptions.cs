using System.Collections.Generic;
using System.Diagnostics;

namespace PutridParrot.Delimited.Data
{
	/// <summary>
	/// Used to set options for the deserializing objects
	/// </summary>
	public class DelimitedDeserializeOptions
	{
		[DebuggerStepThrough]
		public DelimitedDeserializeOptions()
		{
			IgnoreEmptyRows = true;
		}

		/// <summary>
		/// 
		/// </summary>
		public bool UseHeadings { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public bool EnforceRequiredFields { get; set; }
		/// <summary>
		/// Gets/Sets the number of rows to ignore when reading in data
		/// </summary>
		public int IgnoreFirstNRows { get; set; }
		/// <summary>
		/// Gets/Sets the mappings for the object to deserialize to
		/// </summary>
		public IList<FieldReadProperty>? Mappings { get; set; }
		/// <summary>
		/// Gets/Sets whether we should ignore empty rows
		/// </summary>
		public bool IgnoreEmptyRows { get; set; }
	}
}
