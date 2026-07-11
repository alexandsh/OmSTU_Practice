namespace task11;

using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
public interface ICalculator
{
    int Add(int a, int b);
    int Minus(int a, int b);
    int Mul(int a, int b);
    int Div(int a, int b);
}

public class GenerateInRunTime
{
    public static ICalculator GenerateCalculator()
    {
        var CalculatorCode = @"
        using System;
        using task11;

        namespace task11
        {
            public class Calculator : ICalculator
            {
                public int Add(int a, int b) => a + b;
                public int Minus(int a, int b) => a - b;
                public int Mul(int a, int b) => a * b;
                public int Div(int a, int b) => a / b;
            }
        }";
        var syntaxTree = CSharpSyntaxTree.ParseText(CalculatorCode);
        var reference = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location)
        };
        var compilationDll = CSharpCompilation.Create(
            "CalculatorDynamic",
            new[] { syntaxTree },
            reference,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        using var memory = new MemoryStream();
        var result = compilationDll.Emit(memory);
        if (!result.Success)
        {
            var errors = result.Diagnostics
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(d => d.ToString());
            throw new InvalidOperationException(
                "compilation failed:\n" + string.Join("\n", errors));
        }
        memory.Seek(0, SeekOrigin.Begin);
        var assembly = Assembly.Load(memory.ToArray());
        var calculator = (ICalculator)Activator.CreateInstance(assembly.GetType("task11.Calculator")!)!;
        return calculator;
    }
}
