using FileSystemManager.Contexts;
using FileSystemManager.Drivers;
using FileSystemManager.ResultTypes;
using FileSystemManager.ValuesObjects;
using FileSystemManager.Visitors;
using FileSystemManager.Visitors.Nodes;

namespace FileSystemManager.Commands.TreeCommands;

public class TreeListCommand : ICommand
{
    private readonly IDriver _driver;
    private readonly Countable _depth;
    private readonly string _directoryMark;
    private readonly string _fileMark;
    private readonly string _shift;

    public TreeListCommand(IDriver driver, Countable depth, string directoryMark, string fileMark, string shift)
    {
        _driver = driver;
        _depth = depth;
        _directoryMark = directoryMark;
        _fileMark = fileMark;
        _shift = shift;
    }

    public CommandExecutionResult Execute(IContext context)
    {
        if (context.FileSystem is null || context.CurrentPath is null)
            return new CommandExecutionResult.Failure("File System is not connected.");

        var visitor = new FileSystemVisitor(context.FileSystem, _driver, _depth, _directoryMark, _fileMark, _shift);
        var startFrom = new DirectoryNode(context.CurrentPath);

        visitor.Visit(startFrom);

        return new CommandExecutionResult.Success("List of files was successfully wrote.");
    }
}