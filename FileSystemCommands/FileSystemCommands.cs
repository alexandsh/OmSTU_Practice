namespace FileSystemCommands;

using CommandLib;
public class DirectorySizeCommand : ICommand
{
    private string _pathDirectory;
    public long sizeDirectory { get; set; }
    public DirectorySizeCommand(string pathDirectory)
    {
        _pathDirectory = pathDirectory;
    }
    public void Execute()
    {
        sizeDirectory = GetDirectorySize(new DirectoryInfo(_pathDirectory));
    }
    private static long GetDirectorySize(DirectoryInfo dir)
    {
        return dir.EnumerateFiles().Sum((f) => f.Length) + dir.EnumerateDirectories().Sum((d) => GetDirectorySize(d));
    }
}
public class FindFilesCommand : ICommand
{
    public string mask { get; set; }
    public string pathDirectory { get; set; }
    public string[] foundFiles { get; set; } = [];

    public FindFilesCommand(string pathDirectory, string mask)
    {
        this.pathDirectory = pathDirectory;
        this.mask = mask;
    }
    public void Execute()
    {
        foundFiles = Directory.GetFiles(pathDirectory, mask);
    }
}
