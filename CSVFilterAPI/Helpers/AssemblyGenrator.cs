using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace CSVFilters.Helpers;

public static class AssemblyGenrator
{
    public static Assembly CompileCode(string code, string assemblyName)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            new[] { syntaxTree },
            references: new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            },
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        using (var ms = new MemoryStream())
        {
            EmitResult result = compilation.Emit(ms);

            if (!result.Success)
            {
                Console.WriteLine("Compilation failed:");
                foreach (var diagnostic in result.Diagnostics)
                {
                    Console.WriteLine(diagnostic);
                }
                return null;
            }
            

            ms.Seek(0, SeekOrigin.Begin);
            return Assembly.Load(ms.ToArray());
        }
    }

    public static Type GetTypeOfClass(Assembly assembly, string className)
    {
        return assembly.GetType(className);
    }

    public static PropertyInfo[] GetProperties(Type type)
    {
        return type.GetProperties();
    }
}
