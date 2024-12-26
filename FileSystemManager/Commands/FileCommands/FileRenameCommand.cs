using FileSystemManager.Contexts;
using FileSystemManager.ResultTypes;

namespace FileSystemManager.Commands.FileCommands;

public class FileRenameCommand : ICommand
{
    private readonly string _path;
    private readonly string _newName;

    public FileRenameCommand(string path, string newName)
    {
        _path = path;
        _newName = newName;
    }

    public CommandExecutionResult Execute(IContext context)
    {
        if (context.FileSystem is null || context.CurrentPath is null)
            return new CommandExecutionResult.Failure("File System is not connected.");

        string absolutePath = context.FileSystem.GetAbsolutePath(_path, context.CurrentPath);

        FileSystemOperationResult renameResult = context.FileSystem.Rename(absolutePath, _newName);

        return renameResult switch
        {
            FileSystemOperationResult.Success success => new CommandExecutionResult.Success(success.Message),
            FileSystemOperationResult.Failure failure => new CommandExecutionResult.Failure(failure.Message),
            _ => new CommandExecutionResult.Failure("Something went wrong."),
        };
    }
}