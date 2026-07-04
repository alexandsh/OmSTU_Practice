namespace task10tests;

using task10;
using Xunit;

public class PluginLoaderTests
{
    [Fact]
    public void Plugins_Execute_In_Dependency_Order()
    {
        var output = new StringWriter();
        Console.SetOut(output);

        var plugins = PluginLoader.LoadPlugins("../../../../PluginsDll");
        foreach (var plugin in plugins)
            plugin.Execute();

        Assert.Contains("PluginWithoutDep", output.ToString());
        Assert.Contains("PluginWithDep", output.ToString());
    }
}
