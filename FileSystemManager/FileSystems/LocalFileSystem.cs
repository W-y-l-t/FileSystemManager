using FileSystemManager.ResultTypes;

namespace FileSystemManager.FileSystems;

public class LocalFileSystem : IFileSystem
{
    public FileSystemOperationResult Copy(string source, string destination)
    {
        destination = EnsureUniquePath(destination);

        if (File.Exists(source))
        {
            File.Copy(source, destination);
        }
        else if (Directory.Exists(source))
        {
            CopyDirectory(source, destination);
        }
        else
        {
            return new FileSystemOperationResult.Failure("Source path does not exist.");
        }

        return new FileSystemOperationResult.Success($"Copied {source} to {destination}");
    }

    public FileSystemOperationResult Delete(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else if (Directory.Exists(path))
        {
            Directory.Delete(path, recursive: true);
        }
        else
        {
            return new FileSystemOperationResult.Failure("Path does not exist.");
        }

        return new FileSystemOperationResult.Success($"Deleted {path}");
    }

    public FileSystemOperationResult Move(string source, string destination)
    {
        destination = EnsureUniquePath(destination);

        if (File.Exists(source))
        {
            File.Move(source, destination);
        }
        else if (Directory.Exists(source))
        {
            Directory.Move(source, destination);
        }
        else
        {
            return new FileSystemOperationResult.Failure("Source path does not exist.");
        }

        return new FileSystemOperationResult.Success($"Moved {source} to {destination}");
    }

    public FileSystemOperationResult Rename(string path, string newName)
    {
        if (newName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            return new FileSystemOperationResult.Failure("Invalid characters in the new file name.");

        string? parentDirectory = Path.GetDirectoryName(path);
        if (parentDirectory is null)
            return new FileSystemOperationResult.Failure("Parent directory is empty or does not exist.");

        string newPath;

        if (File.Exists(path))
        {
            string extension = Path.GetExtension(path);
            if (!newName.EndsWith(extension))
                newName += extension;

            newPath = EnsureUniquePath(Path.Combine(parentDirectory, newName));

            File.Move(path, newPath);
        }
        else if (Directory.Exists(path))
        {
            newPath = EnsureUniquePath(Path.Combine(parentDirectory, newName));

            Directory.Move(path, newPath);
        }
        else
        {
            return new FileSystemOperationResult.Failure("Path does not exist.");
        }

        return new FileSystemOperationResult.Success($"File {path} renamed to {newPath}");
    }

    public bool IsPathFullyQualified(string path)
    {
        return Path.IsPathFullyQualified(path);
    }

    public string GetAbsolutePath(string path)
    {
        return GetAbsolutePath(path, Directory.GetCurrentDirectory());
    }

    public string GetAbsolutePath(string path, string basePath)
    {
        return IsPathFullyQualified(path) ? path : Path.GetFullPath(path, basePath);
    }

    public IEnumerable<string> GetDirectories(string path)
    {
        return Directory.GetDirectories(path);
    }

    public IEnumerable<string> GetFiles(string path)
    {
        return Directory.GetFiles(path);
    }

    public StreamReader GetContent(string path)
    {
        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);

        return new StreamReader(stream);
    }

    public bool IsFullyQualified(string path)
    {
        return Path.IsPathFullyQualified(path);
    }

    private static void CopyDirectory(string sourceDirectory, string destinationDirectory)
    {
        Directory.CreateDirectory(destinationDirectory);

        foreach (string filePath in Directory.GetFiles(sourceDirectory))
        {
            string fileName = Path.GetFileName(filePath);
            string destinationFilePath = Path.Combine(destinationDirectory, fileName);

            File.Copy(filePath, destinationFilePath, overwrite: true);
        }

        foreach (string subDirectory in Directory.GetDirectories(sourceDirectory))
        {
            string subDirectoryName = Path.GetFileName(subDirectory);
            string destinationSubDirectoryPath = Path.Combine(destinationDirectory, subDirectoryName);

            CopyDirectory(subDirectory, destinationSubDirectoryPath);
        }
    }

    private static string EnsureUniquePath(string path)
    {
        string directory = Path.GetDirectoryName(path) ?? string.Empty;
        string fileName = Path.GetFileNameWithoutExtension(path);
        string extension = Path.GetExtension(path);

        string uniquePath = path;
        int counter = 1;

        while (File.Exists(uniquePath) || Directory.Exists(uniquePath))
        {
            uniquePath = Path.Combine(directory, $"{fileName}({counter++}){extension}");
        }

        return uniquePath;
    }
}