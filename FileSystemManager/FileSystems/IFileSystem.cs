using FileSystemManager.ResultTypes;

namespace FileSystemManager.FileSystems;

public interface IFileSystem
{
    FileSystemOperationResult Copy(string source, string destination);

    FileSystemOperationResult Delete(string path);

    FileSystemOperationResult Move(string source, string destination);

    FileSystemOperationResult Rename(string path, string newName);

    string GetAbsolutePath(string path);

    string GetAbsolutePath(string path, string basePath);

    IEnumerable<string> GetDirectories(string path);

    IEnumerable<string> GetFiles(string path);

    StreamReader GetContent(string path);

    bool IsFullyQualified(string path);
}