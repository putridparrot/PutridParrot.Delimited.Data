using PowerArgs;

namespace DelimitedCodeGenerator
{
	public class Arguments
	{
		[
		ArgDescription("The input file name"),
		ArgRequired(PromptIfMissing = true),
		ArgExistingFile,
		ArgShortcut("i")
		]
		public string InputFilename { get; set; }

		[
		ArgDescription("The output source file name"),
		ArgShortcut("o")
		]
		public string OutputFilename { get; set; }

		[
		ArgDescription("Does the file use the first row as the column headings"),
		ArgShortcut("h")
		]
		public bool UseHeaders { get; set; }

		[
		ArgDescription("The delimiter used"),
		ArgShortcut("d"),
		DefaultValue(',')
		]
		public char Delimiter { get; set; }

		[
		ArgDescription("The qualifier used"),
		ArgShortcut("q"),
		DefaultValue('"')
		]
		public char Qualifier { get; set; }

		[
		ArgDescription("The qualifier used"),
		ArgShortcut("qa"),
		DefaultValue(false)
		]
		public bool QualifierAll { get; set; }
	}
}
