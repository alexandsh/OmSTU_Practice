namespace PluginWithoutDep;

using PluginSetup;

[PluginLoad]
public class PluginWithoutDep : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("PluginWithoutDep");
    }
}
