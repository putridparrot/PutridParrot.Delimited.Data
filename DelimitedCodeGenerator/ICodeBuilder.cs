namespace DelimitedCodeGenerator
{
	public interface ICodeBuilder
	{
		string Generate(HeadingType[] headings, bool expectHeader);
	}
}
