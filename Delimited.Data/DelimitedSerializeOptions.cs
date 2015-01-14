using System.Collections.Generic;

namespace Delimited.Data
{
	/// <summary>
	/// Used for setting the serialization options
	/// </summary>
	public class DelimitedSerializeOptions
	{
		/// <summary>
		/// Gets/Sets the mappings for how data should be serialized
		/// </summary>
		public IList<FieldWriteProperty> Mappings { get; set; }
		/// <summary>
		/// Gets/Sets whether we should include headers in the serialized data
		/// </summary>
		public bool IncludeHeadings { get; set; }
	}
}
