using FileSystemManager.FileSystems;

namespace FileSystemManager.Contexts;

public interface IContext
{
    string? CurrentPath { get; }

    IFileSystem? FileSystem { get; }

    void ClearContext();

    void SetFileSystem(IFileSystem fileSystem);

    void SetCurrentPath(string currentPath);
}