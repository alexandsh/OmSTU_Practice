namespace task07;

using System.Reflection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; }

    public DisplayNameAttribute(string name) 
    {
        DisplayName = name;    
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class VersionAttribute : Attribute
{
    public int Major { get; }
    public int Minor { get; }

    public VersionAttribute(int major, int minor)
    {
        Major = major;
        Minor = minor;
    }
}

[DisplayName("Пример класса")]
[Version(1, 0)]
public class SampleClass
{
    [DisplayName("Числовое свойство")]
    public int Number { get; set; }

    [DisplayName("Тестовый метод")]
    public void TestMethod() {}

}

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        var classDisplayName = type.GetCustomAttribute<DisplayNameAttribute>();
        if (classDisplayName != null)
        {
            Console.WriteLine(classDisplayName.DisplayName);
        }
        
        var classVersion = type.GetCustomAttribute<VersionAttribute>();
        if (classVersion != null)
        {
            Console.WriteLine($"{classVersion.Major}.{classVersion.Minor}");
        }

        foreach (var method in type.GetMethods())
        {
            if (method.DeclaringType == type) 
            {
                var methodDisplayName = method.GetCustomAttribute<DisplayNameAttribute>();
                if (methodDisplayName != null)
                {
                    Console.WriteLine($"{method.Name}: {methodDisplayName.DisplayName}");
                }
            }
        }

        foreach (var prop in type.GetProperties())
        {
            var propDisplayName = prop.GetCustomAttribute<DisplayNameAttribute>();
            if (propDisplayName != null)
            {
                Console.WriteLine($"{prop.Name}: {propDisplayName.DisplayName}");
            }
        }
    }
}
