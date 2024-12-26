using FileSystemManager.Contexts;
using FileSystemManager.ResultTypes;

namespace FileSystemManager.Commands.TreeCommands;

public class TreeGoToCommand : ICommand
{
    private readonly string _path;

    public TreeGoToCommand(string path)
    {
        _path = path;
    }

    public CommandExecutionResult Execute(IContext context)
    {
        if (context.FileSystem is null || context.CurrentPath is null)
            return new CommandExecutionResult.Failure("File System is not connected.");

        string absolutePath = context.FileSystem.GetAbsolutePath(_path, context.CurrentPath);

        context.SetCurrentPath(absolutePath);

        return new CommandExecutionResult.Success($"Current path is {absolutePath}.");
    }
}