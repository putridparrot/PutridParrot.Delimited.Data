using System;
using System.Globalization;
using System.Text;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;

namespace DelimitedCodeGenerator
{
	public class CSharpCodeBuilder : ICodeBuilder
	{
		private static string CreateValidPropertyName(string name, CodeDomProvider provider)
		{
			//Compliant with item 2.4.2 of the C# specification
			var regex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]");
			return provider.CreateEscapedIdentifier(regex.Replace(name, ""));
		}

		private static string ToTitleCase(TextInfo ti, string name)
		{
			var converted = name.Replace('_', ' ').Replace('-', ' ').ToLower();
			return ti.ToTitleCase(converted);
		}

		public string Generate(HeadingType[] headings, bool expectHeader)
		{
			using (var provider = CodeDomProvider.CreateProvider("CSharp"))
			{
				var classBuilder = new StringBuilder();

				classBuilder.AppendLine("using System;");
				classBuilder.AppendLine("using Delimited.Data;");
				classBuilder.AppendLine("using Delimited.Data.Attributes;");
				classBuilder.AppendLine();
				classBuilder.AppendLine("class DelimitedGeneratedData");
				classBuilder.AppendLine("{");

				var ti = CultureInfo.CurrentCulture.TextInfo;

				var i = 0;
				foreach (var headingType in headings)
				{
					if (expectHeader)
					{
						// use heading string
						classBuilder.AppendLine(String.Format("\t[DelimitedFieldRead(\"{0}\")]", headingType.Heading));
						classBuilder.AppendLine(String.Format("\t[DelimitedFieldWrite(\"{0}\")]", headingType.Heading));
					}
					else
					{
						// use column index
						classBuilder.AppendLine(String.Format("\t[DelimitedFieldRead({0})]", i));
						classBuilder.AppendLine(String.Format("\t[DelimitedFieldWrite({0})]", i));
						i++;
					}
					classBuilder.AppendLine(String.Format("\tpublic {0} {1} {{ get; set; }}", headingType.Type.Name,
						CreateValidPropertyName(ToTitleCase(ti, headingType.Heading), provider)));
				}

				classBuilder.AppendLine("}");
				return classBuilder.ToString();
			}
		}
	}
}
