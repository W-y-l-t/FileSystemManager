namespace FileSystemManager.FileSystems.InMemoryFileSystems;

public class FileSystemNode
{
    public FileSystemNode(string path, FileSystemNodeType type, string content)
    {
        Path = path;
        Type = type;
        Content = content;
    }

    public string Path { get; set; }

    public FileSystemNodeType Type { get; }

    public string Content { get; }

    public FileSystemNode Clone()
    {
        return new FileSystemNode(Path, Type, Content);
    }
}