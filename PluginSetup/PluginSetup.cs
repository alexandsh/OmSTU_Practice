namespace PluginSetup;

public interface IPlugin
{
    void Execute();
}

[AttributeUsage(AttributeTargets.Class)]
public class PluginLoadAttribute : Attribute
{
    public string[] Depends { get; }
    public PluginLoadAttribute(params string[] depends)
    {
        Depends = depends;
    }
}
