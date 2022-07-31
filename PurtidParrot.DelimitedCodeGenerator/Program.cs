using System;
using System.IO;
using PowerArgs;

namespace DelimitedCodeGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var arguments = Args.Parse<Arguments>(args);

				var codeBuilder = new CSharpCodeBuilder();
				var code = Parser.Parse(arguments.InputFilename, arguments.UseHeaders, 
					arguments.Delimiter, arguments.Qualifier, 
					arguments.QualifierAll, codeBuilder);
				if (!String.IsNullOrEmpty(arguments.OutputFilename))
				{
					using (var sw = new StreamWriter(File.OpenWrite(arguments.OutputFilename)))
					{
						sw.Write(code);
					}
				}
				else
				{
					Console.WriteLine(code);
				}
			}
			catch (ArgException ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ArgUsage.GetStyledUsage<Arguments>());
			}
		}
	}
}
