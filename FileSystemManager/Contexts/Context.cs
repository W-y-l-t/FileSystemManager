using FileSystemManager.FileSystems;

namespace FileSystemManager.Contexts;

public class Context : IContext
{
    public Context() : this(null) { }

    public Context(IFileSystem? fileSystem)
    {
        FileSystem = fileSystem;
    }

    public string? CurrentPath { get; private set; }

    public IFileSystem? FileSystem { get; private set; }

    public void ClearContext()
    {
        CurrentPath = null;
        FileSystem = null;
    }

    public void SetFileSystem(IFileSystem fileSystem)
    {
        FileSystem = fileSystem;
    }

    public void SetCurrentPath(string currentPath)
    {
        CurrentPath = currentPath;
    }
}