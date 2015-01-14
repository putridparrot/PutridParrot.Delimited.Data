using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using Delimited.Data;

namespace DelimitedCodeGeneratorTests
{
	[ExcludeFromCodeCoverage]
	internal static class ReflectionUtils
	{
		public static Assembly GenerateAssembly(string sourceCode, string[] references)
		{
			CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
			var cp = new CompilerParameters
			{
				GenerateInMemory = true, 
				TreatWarningsAsErrors = false
			};

			if (references != null)
			{
				foreach (string r in references)
				{
					cp.ReferencedAssemblies.Add(r);
				}
			}
			CompilerResults cr = provider.CompileAssemblyFromSource(cp, sourceCode);

			if (cr.Errors.Count > 0)
			{
				var sb = new StringBuilder();
				foreach (CompilerError err in cr.Errors)
				{
					sb.AppendLine(err.ErrorText);
				}

				throw new SyntaxErrorException(sb.ToString());
			}

			return cr.CompiledAssembly;
		}

		public static IEnumerable InvokeDeserializer(Assembly generatedAssembly, string data, DelimitedOptions delimitedOptions)
		{
			var dr = new DelimitedSeparatedReader(delimitedOptions);
			var opt = new DelimitedDeserializeOptions { UseHeadings = true };
			object[] parameters = { dr, data, opt };
			Type[] arguments = { typeof(IDelimitedSeparatedReader), typeof(string), typeof(DelimitedDeserializeOptions) };

			Type type = generatedAssembly.GetType("DelimitedGeneratedData");

			Type dataserializer = typeof(DelimitedSerializer<>);
			Type typedSerializer = dataserializer.MakeGenericType(type);
			MethodInfo mi = typedSerializer.GetMethod("Deserialize", BindingFlags.Public | BindingFlags.Static, null, arguments, null);
			return mi.Invoke(null, parameters) as IEnumerable;
		}
	}

}
