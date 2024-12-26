using FileSystemManager.Contexts;
using FileSystemManager.FileSystems.InMemoryFileSystems;
using FileSystemManager.ResultTypes;

namespace FileSystemManager.Commands.ConnectionCommands.ConnectCommands;

public class ConnectInMemoryCommand : ICommand
{
    private readonly string _path;

    public ConnectInMemoryCommand(string path)
    {
        _path = path;
    }

    public CommandExecutionResult Execute(IContext context)
    {
        var inMemoryFileSystem = new InMemoryFileSystem();

        if (!inMemoryFileSystem.IsFullyQualified(_path))
            return new CommandExecutionResult.Failure("Path is not fully qualified.");

        if (context.FileSystem is not null)
            return new CommandExecutionResult.Failure("File System is already connected.");

        context.SetFileSystem(inMemoryFileSystem);
        context.SetCurrentPath(_path);

        return new CommandExecutionResult.Success("Successfully connected to in-memory file system.");
    }
}