using FileSystemManager.Drivers;
using FileSystemManager.FileSystems;
using FileSystemManager.ValuesObjects;
using FileSystemManager.Visitors.Nodes;
using System.Text;

namespace FileSystemManager.Visitors;

public class FileSystemVisitor : IVisitor
{
    private readonly Countable _currentDepth;
    private readonly IFileSystem _fileSystem;
    private readonly IDriver _driver;
    private readonly Countable _maxDepth;
    private readonly string _directoryMark;
    private readonly string _fileMark;
    private readonly string _shift;

    public FileSystemVisitor(
        IFileSystem fileSystem,
        IDriver driver,
        Countable depth,
        string directoryMark,
        string fileMark,
        string shift)
    {
        _fileSystem = fileSystem;
        _driver = driver;
        _maxDepth = depth;
        _directoryMark = directoryMark;
        _fileMark = fileMark;
        _shift = shift;
        _currentDepth = new Countable(0);
    }

    public void Visit(FileNode fileNode)
    {
        if (_currentDepth.Value > _maxDepth.Value)
            return;

        _driver.Print(ApplyFormatting(fileNode, _currentDepth));
    }

    public void Visit(DirectoryNode directoryNode)
    {
        if (_currentDepth.Value > _maxDepth.Value)
            return;

        _driver.Print(ApplyFormatting(directoryNode, _currentDepth));

        try
        {
            _currentDepth.Value += 1;

            foreach (string file in _fileSystem.GetFiles(directoryNode.Path))
            {
                Visit(new FileNode(file));
            }

            foreach (string subDirectory in _fileSystem.GetDirectories(directoryNode.Path))
            {
                Visit(new DirectoryNode(subDirectory));
            }

            _currentDepth.Value -= 1;
        }
        catch (UnauthorizedAccessException)
        {
            _driver.Print($"{MakeShift(_currentDepth)}" +
                          $"Access denied to: {directoryNode.Path}");
        }
        catch (IOException ex)
        {
            _driver.Print($"{MakeShift(_currentDepth)}" +
                          $"Error accessing: {directoryNode.Path}. Message: {ex.Message}");
        }
    }

    private static string GetName(INode node)
    {
        string stringPath = node.Path;

        return stringPath[(stringPath.LastIndexOf(Path.DirectorySeparatorChar) + 1)..];
    }

    private string MakeShift(Countable shiftCount)
    {
        var builder = new StringBuilder();

        for (int i = 0; i < shiftCount.Value; i += 1)
            builder.Append(_shift);

        return builder.ToString();
    }

    private string ApplyFormatting(INode node, Countable shiftCount)
    {
        var builder = new StringBuilder();

        builder.Append(MakeShift(shiftCount));

        switch (node)
        {
            case FileNode:
                builder.Append(_fileMark);
                break;
            case DirectoryNode:
                builder.Append(_directoryMark);
                break;
            default:
                builder.Append("May be it's a cat? ");
                break;
        }

        builder.Append(GetName(node));

        return builder.ToString();
    }
}