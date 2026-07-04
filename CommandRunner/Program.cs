using System.Reflection;


var assembly = Assembly.LoadFrom("../../../../FileSystemCommands/bin/Debug/net9.0/FileSystemCommands.dll");

var directorySizeCommandType = assembly.GetType("FileSystemCommands.DirectorySizeCommand");
var directorySizeCommandLaunch = directorySizeCommandType?.GetMethod("Execute", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
var directorySizeCommandCopy = Activator.CreateInstance(directorySizeCommandType!, new object[] { "../../../../FileSystemCommands"});

directorySizeCommandLaunch?.Invoke(directorySizeCommandCopy, null);

var FindFilesCommandType = assembly.GetType("FileSystemCommands.FindFilesCommand");
var FindFilesCommandLaunch = FindFilesCommandType?.GetMethod("Execute", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
var FindFilesCommandCopy = Activator.CreateInstance(FindFilesCommandType!, new object[] { "../../../../FileSystemCommands" , "*.cs" });

FindFilesCommandLaunch?.Invoke(FindFilesCommandCopy, null);