using FileSystemManager.Visitors.Nodes;

namespace FileSystemManager.Visitors;

public interface IVisitor
{
    void Visit(FileNode fileNode);

    void Visit(DirectoryNode directoryNode);
}