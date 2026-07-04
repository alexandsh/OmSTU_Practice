namespace task10;

using System.Reflection;
using PluginSetup;

public class PluginLoader
{
    public static List<IPlugin> LoadPlugins(string dir)
    {
        var plugins = new Dictionary<string, (Type type, string[] deps)>();
        var result = new List<IPlugin>();

        foreach (var dll in Directory.GetFiles(dir, "*.dll"))
        {
            var assembly = Assembly.LoadFrom(dll);
            foreach (var type in assembly.GetTypes())
            {
                var attr = type.GetCustomAttribute<PluginLoadAttribute>();
                if (attr != null && typeof(IPlugin).IsAssignableFrom(type))
                {
                    plugins[type.Name] = (type, attr.Depends != null ? attr.Depends : Array.Empty<string>());
                }
            }
        }

        void LoadWithDeps(string name)
        {
            foreach (var dep in plugins[name].deps)
            {
                LoadWithDeps(dep);
            }
            var instance = (IPlugin)Activator.CreateInstance(plugins[name].type)!;
            result.Add(instance);
        }

        foreach (var name in plugins.Keys)
        {
            LoadWithDeps(name);
        }

        return result;
    }
}
