using FileSystemManager.Contexts;
using FileSystemManager.FileSystems;
using FileSystemManager.ResultTypes;

namespace FileSystemManager.Commands.ConnectionCommands.ConnectCommands;

public class ConnectLocalCommand : ICommand
{
    private readonly string _path;

    public ConnectLocalCommand(string path)
    {
        _path = path;
    }

    public CommandExecutionResult Execute(IContext context)
    {
        var localFileSystem = new LocalFileSystem();

        if (!localFileSystem.IsFullyQualified(_path))
            return new CommandExecutionResult.Failure("Path is not fully qualified.");

        if (context.FileSystem is not null)
            return new CommandExecutionResult.Failure("File System is already connected.");

        context.SetFileSystem(localFileSystem);
        context.SetCurrentPath(_path);

        return new CommandExecutionResult.Success("Successfully connected to local file system");
    }
}