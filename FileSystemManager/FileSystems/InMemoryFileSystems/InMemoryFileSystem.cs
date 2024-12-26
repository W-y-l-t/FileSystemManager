using FileSystemManager.ResultTypes;
using FileSystemManager.ValuesObjects;
using System.Text;

namespace FileSystemManager.FileSystems.InMemoryFileSystems;

public class InMemoryFileSystem : IFileSystem
{
    private readonly Dictionary<EquatableString, FileSystemNode> _fileSystem = [];

    public FileSystemOperationResult Copy(string source, string destination)
    {
        var sourceWrapper = new EquatableString(source);

        if (!_fileSystem.TryGetValue(sourceWrapper, out FileSystemNode? sourceNode))
            return new FileSystemOperationResult.Failure("Source path does not exist.");

        destination = EnsureUniquePath(destination);
        var destinationWrapper = new EquatableString(destination);

        FileSystemNode destinationNode = sourceNode.Clone();
        destinationNode.Path = destination;

        _fileSystem[destinationWrapper] = destinationNode;

        return new FileSystemOperationResult.Success($"Copied {source} to {destination}");
    }

    public FileSystemOperationResult Delete(string path)
    {
        var pathWrapper = new EquatableString(path);

        if (!_fileSystem.TryGetValue(pathWrapper, out FileSystemNode? pathNode))
            return new FileSystemOperationResult.Failure("Path does not exist.");

        if (pathNode.Type == FileSystemNodeType.Directory)
        {
            IEnumerable<string> children =
                _fileSystem.Keys
                    .Where(p => p.Value.StartsWith(path + "/"))
                    .Select(p => p.Value);

            foreach (string child in children)
            {
                Delete(child);
            }
        }

        _fileSystem.Remove(pathWrapper);

        return new FileSystemOperationResult.Success($"Deleted {path}");
    }

    public FileSystemOperationResult Move(string source, string destination)
    {
        var sourceWrapper = new EquatableString(source);

        if (!_fileSystem.ContainsKey(sourceWrapper))
            return new FileSystemOperationResult.Failure("Source path does not exist.");

        destination = EnsureUniquePath(destination);

        FileSystemOperationResult copyResult = Copy(source, destination);
        if (copyResult is FileSystemOperationResult.Failure)
            return copyResult;

        Delete(source);
        return new FileSystemOperationResult.Success($"Moved {source} to {destination}");
    }

    public FileSystemOperationResult Rename(string path, string newName)
    {
        var pathWrapper = new EquatableString(path);

        if (!_fileSystem.ContainsKey(pathWrapper))
            return new FileSystemOperationResult.Failure("Path does not exist.");

        if (ContainsInvalidCharacters(newName))
            return new FileSystemOperationResult.Failure("Invalid characters in the new file name.");

        string? parentDirectory = GetParentDirectory(path);
        if (parentDirectory is null)
            return new FileSystemOperationResult.Failure("Parent directory is invalid.");

        string newPath = CombinePath(parentDirectory, newName);
        newPath = EnsureUniquePath(newPath);

        FileSystemNode pathNode = _fileSystem[pathWrapper];
        _fileSystem.Remove(pathWrapper);

        var newPathWrapper = new EquatableString(newPath);

        pathNode.Path = newPath;
        _fileSystem[newPathWrapper] = pathNode;

        return new FileSystemOperationResult.Success($"File {path} renamed to {newPath}");
    }

    public string GetAbsolutePath(string path)
    {
        return GetAbsolutePath(path, "/");
    }

    public string GetAbsolutePath(string path, string basePath)
    {
        if (IsFullyQualified(path))
            return path;

        string combined = CombinePath(basePath, path);
        return NormalizePath(combined);
    }

    public IEnumerable<string> GetDirectories(string path)
    {
        var pathWrapper = new EquatableString(path);

        if (!_fileSystem.TryGetValue(pathWrapper, out FileSystemNode? pathNode))
            return [];

        if (pathNode.Type != FileSystemNodeType.Directory)
            return [];

        string normalizedPath = NormalizePath(path);

        return _fileSystem.Keys
            .Where(p => IsDirectChildDirectory(normalizedPath, p.Value))
            .Select(p => p.Value);
    }

    public IEnumerable<string> GetFiles(string path)
    {
        var pathWrapper = new EquatableString(path);

        if (!_fileSystem.TryGetValue(pathWrapper, out FileSystemNode? pathNode))
            return [];

        if (pathNode.Type != FileSystemNodeType.Directory)
            return [];

        string normalizedPath = NormalizePath(path);

        return _fileSystem.Keys
            .Where(p => IsDirectChildFile(normalizedPath, p.Value))
            .Select(p => p.Value);
    }

    public StreamReader GetContent(string path)
    {
        var pathWrapper = new EquatableString(path);

        if (!_fileSystem.TryGetValue(pathWrapper, out FileSystemNode? pathNode))
            return StreamReader.Null;

        if (pathNode.Type != FileSystemNodeType.File)
            return StreamReader.Null;

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(pathNode.Content));

        return new StreamReader(stream);
    }

    public void CreateNode(string path, FileSystemNodeType type, string content = "")
    {
        var pathWrapper = new EquatableString(path);

        if (_fileSystem.ContainsKey(pathWrapper))
            return;

        _fileSystem[pathWrapper] = new FileSystemNode(path, type, content);
    }

    public bool IsFullyQualified(string path)
    {
        return path.StartsWith('/');
    }

    private string EnsureUniquePath(string path)
    {
        string directory = GetParentDirectory(path) ?? string.Empty;
        string fileName = GetFileNameWithoutExtension(path);
        string extension = GetExtension(path);

        var pathWrapper = new EquatableString(path);
        int counter = 1;

        while (_fileSystem.ContainsKey(pathWrapper))
        {
            pathWrapper =
                new EquatableString(CombinePath(directory, $"{fileName}({counter++}){extension}"));
        }

        return pathWrapper.Value;
    }

    private string NormalizePath(string path)
    {
        string[] segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var stack = new Stack<string>();

        foreach (string segment in segments)
        {
            if (segment == ".." && stack.Count > 0)
                stack.Pop();
            else if (segment != "." && segment != "..")
                stack.Push(segment);
        }

        return "/" + string.Join("/", stack.Reverse());
    }

    private string CombinePath(string basePath, string relativePath)
    {
        if (IsFullyQualified(relativePath))
            return relativePath;

        if (!basePath.EndsWith('/'))
            basePath += "/";

        return basePath + relativePath.TrimStart('/');
    }

    private string? GetParentDirectory(string path)
    {
        if (path == "/")
            return null;

        int lastSlashIndex = path.LastIndexOf('/');

        return lastSlashIndex > 0 ? path[..lastSlashIndex] : "/";
    }

    private string GetFileNameWithoutExtension(string path)
    {
        string fileName = GetFileName(path);
        int extensionIndex = fileName.LastIndexOf('.');

        return extensionIndex > 0 ? fileName[..extensionIndex] : fileName;
    }

    private string GetFileName(string path)
    {
        int lastSlashIndex = path.LastIndexOf('/');

        return lastSlashIndex >= 0 ? path[(lastSlashIndex + 1)..] : path;
    }

    private string GetExtension(string path)
    {
        int lastDotIndex = path.LastIndexOf('.');

        return lastDotIndex >= 0 ? path[lastDotIndex..] : string.Empty;
    }

    private bool ContainsInvalidCharacters(string name)
    {
        char[] invalidChars = ['/', '\\', ':', '*', '?', '"', '<', '>', '|'];

        return name.IndexOfAny(invalidChars) >= 0;
    }

    private bool IsDirectChildDirectory(string parentPath, string potentialChildPath)
    {
        var potentialChildPathWrapper = new EquatableString(potentialChildPath);

        if (!_fileSystem.TryGetValue(potentialChildPathWrapper, out FileSystemNode? potentialChildPathNode))
            return false;

        if (potentialChildPathNode.Type != FileSystemNodeType.Directory)
            return false;

        string normalizedChild = NormalizePath(potentialChildPath);

        return IsDirectChild(parentPath, normalizedChild);
    }

    private bool IsDirectChildFile(string parentPath, string potentialChildPath)
    {
        var potentialChildPathWrapper = new EquatableString(potentialChildPath);

        if (!_fileSystem.TryGetValue(potentialChildPathWrapper, out FileSystemNode? potentialChildPathNode))
            return false;

        if (potentialChildPathNode.Type != FileSystemNodeType.File)
            return false;

        string normalizedChild = NormalizePath(potentialChildPath);

        return IsDirectChild(parentPath, normalizedChild);
    }

    private bool IsDirectChild(string parentPath, string childPath)
    {
        if (!childPath.StartsWith(parentPath))
            return false;

        string remainingPath = childPath[parentPath.Length..].Trim('/');

        return remainingPath.Split('/').Length == 1 && !string.IsNullOrEmpty(remainingPath);
    }
}