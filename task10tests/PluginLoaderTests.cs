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

    [Fact]
    public void Dependency_Loads_Before_Dependent()
    {
        var plugins = PluginLoader.LoadPlugins("../../../../PluginsDll");
        var names = plugins.Select(p => p.GetType().Name).ToList();

        var withoutDep = names.IndexOf("PluginWithoutDep");
        var withDep = names.IndexOf("PluginWithDep");

        Assert.True(withoutDep >= 0, "PluginWithoutDep not loaded");
        Assert.True(withDep >= 0, "PluginWithDep not loaded");
        Assert.True(withoutDep < withDep, "dependency must load before dependent");

        Assert.Equal(names.Count, names.Distinct().Count());
    }
}
