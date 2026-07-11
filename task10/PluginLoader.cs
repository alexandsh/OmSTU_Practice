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
            Assembly assembly;
            Type[] types;
            try
            {
                assembly = Assembly.LoadFrom(dll);
                types = assembly.GetTypes();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error:'{Path.GetFileName(dll)}': {ex.Message}");
                continue;
            }

            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<PluginLoadAttribute>();
                if (attr != null && typeof(IPlugin).IsAssignableFrom(type))
                {
                    plugins[type.Name] = (type, attr.Depends != null ? attr.Depends : Array.Empty<string>());
                }
            }
        }

        var loaded = new HashSet<string>();
        var visiting = new HashSet<string>();

        void LoadWithDeps(string name)
        {
            if (loaded.Contains(name))
                return;
            if (!plugins.ContainsKey(name))
                throw new InvalidOperationException($"Missing dependency: '{name}'");
            if (!visiting.Add(name))
                throw new InvalidOperationException($"Cyclic dependency detected: '{name}'");

            foreach (var dep in plugins[name].deps)
            {
                LoadWithDeps(dep);
            }

            var instance = (IPlugin)Activator.CreateInstance(plugins[name].type)!;
            result.Add(instance);
            loaded.Add(name);
            visiting.Remove(name);
        }

        foreach (var name in plugins.Keys)
        {
            LoadWithDeps(name);
        }

        return result;
    }
}
