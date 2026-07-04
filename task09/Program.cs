namespace task09;

using System.Reflection;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(args[0]);
        var assembly = Assembly.LoadFrom(args[0]);

        foreach (var type in assembly.GetTypes())
        {
            if (type.IsClass)
            {
                Console.WriteLine($"class {type.Name}");
                foreach (var attr in type.GetCustomAttributes())
                {
                    Console.WriteLine($"attrs {attr}");
                }
                foreach (var constructor in type.GetConstructors())
                {
                    Console.WriteLine($"constructor {constructor.Name}");
                    foreach (var param in constructor.GetParameters())
                    {
                        Console.WriteLine($"constructor {constructor.Name} param {param.ParameterType} - {param.Name}");
                    }
                }
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                {
                    Console.WriteLine($"methods {method.Name}");
                    foreach (var param in method.GetParameters())
                    {
                        Console.WriteLine($"method {method.Name} param {param.ParameterType} - {param.Name}");
                    }
                }
            }
        }
    }
}