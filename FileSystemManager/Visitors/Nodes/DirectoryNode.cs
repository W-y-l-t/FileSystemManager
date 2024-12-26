namespace FileSystemManager.Visitors.Nodes;

public class DirectoryNode : INode
{
    public DirectoryNode(string path)
    {
        Path = path;
    }

    public string Path { get; }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}