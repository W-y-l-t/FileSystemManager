using FileSystemManager.Contexts;
using FileSystemManager.ResultTypes;

namespace FileSystemManager.Commands.ConnectionCommands;

public class DisconnectCommand : ICommand
{
    public CommandExecutionResult Execute(IContext context)
    {
        if (context.FileSystem is null || context.CurrentPath is null)
            return new CommandExecutionResult.Failure("File System is not connected.");

        string fullFileSystemType = context.FileSystem.GetType().ToString();
        string shortFileSystemType = fullFileSystemType[(fullFileSystemType.LastIndexOf('.') + 1)..];
        context.ClearContext();

        return new CommandExecutionResult.Success($"Successfully disconnected from {shortFileSystemType}.");
    }
}