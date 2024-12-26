namespace FileSystemManager.Visitors.Nodes;

public class FileNode : INode
{
    public FileNode(string path)
    {
        Path = path;
    }

    public string Path { get; }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}