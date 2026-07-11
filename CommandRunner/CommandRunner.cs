using System.Reflection;


const string dllPath = "../../../../FileSystemCommands/bin/Debug/net9.0/FileSystemCommands.dll";

Assembly assembly;
try
{
    assembly = Assembly.LoadFrom(dllPath);
}
catch (FileNotFoundException)
{
    Console.Error.WriteLine($"error: '{dllPath}'");
    return 1;
}
catch (FileLoadException ex)
{
    Console.Error.WriteLine($"error: '{dllPath}': {ex.Message}");
    return 1;
}
catch (BadImageFormatException)
{
    Console.Error.WriteLine($"error: '{dllPath}'");
    return 1;
}

var directorySizeCommandType = assembly.GetType("FileSystemCommands.DirectorySizeCommand");
var directorySizeCommandLaunch = directorySizeCommandType?.GetMethod("Execute", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
var directorySizeCommandCopy = Activator.CreateInstance(directorySizeCommandType!, new object[] { "../../../../FileSystemCommands"});

directorySizeCommandLaunch?.Invoke(directorySizeCommandCopy, null);

var FindFilesCommandType = assembly.GetType("FileSystemCommands.FindFilesCommand");
var FindFilesCommandLaunch = FindFilesCommandType?.GetMethod("Execute", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
var FindFilesCommandCopy = Activator.CreateInstance(FindFilesCommandType!, new object[] { "../../../../FileSystemCommands" , "*.cs" });

FindFilesCommandLaunch?.Invoke(FindFilesCommandCopy, null);

return 0;
