namespace task09tests;

using System.Reflection;
using Xunit;
using task09;

public class Task09Tests
{
    [Fact]
    public void PrintsTypeInfo_FromTask07()
    {
        var output = new StringWriter();
        Console.SetOut(output);

        var assemblyPath = typeof(task07.SampleClass).Assembly.Location;

        task09.Program.Main(new string[] { assemblyPath });

        Assert.Contains("class SampleClass", output.ToString());
        Assert.Contains("attrs task07.DisplayNameAttribute", output.ToString());
        Assert.Contains("attrs task07.VersionAttribute", output.ToString());
        Assert.Contains("methods TestMethod", output.ToString());
        Assert.Contains("methods get_Number", output.ToString());
    }
}
