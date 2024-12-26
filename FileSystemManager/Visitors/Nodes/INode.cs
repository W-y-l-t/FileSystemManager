namespace FileSystemManager.Visitors.Nodes;

public interface INode
{
    string Path { get; }

    void Accept(IVisitor visitor);
}