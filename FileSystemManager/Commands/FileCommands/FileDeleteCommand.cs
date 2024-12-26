using FileSystemManager.Contexts;
using FileSystemManager.ResultTypes;

namespace FileSystemManager.Commands.FileCommands;

public class FileDeleteCommand : ICommand
{
    private readonly string _path;

    public FileDeleteCommand(string path)
    {
        _path = path;
    }

    public CommandExecutionResult Execute(IContext context)
    {
        if (context.FileSystem is null || context.CurrentPath is null)
            return new CommandExecutionResult.Failure("File System is not connected.");

        string absolutePath = context.FileSystem.GetAbsolutePath(_path, context.CurrentPath);

        FileSystemOperationResult deleteResult = context.FileSystem.Delete(absolutePath);

        return deleteResult switch
        {
            FileSystemOperationResult.Success success => new CommandExecutionResult.Success(success.Message),
            FileSystemOperationResult.Failure failure => new CommandExecutionResult.Failure(failure.Message),
            _ => new CommandExecutionResult.Failure("Something went wrong."),
        };
    }
}