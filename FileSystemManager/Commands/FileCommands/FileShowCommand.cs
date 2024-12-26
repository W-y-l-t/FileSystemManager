using FileSystemManager.Contexts;
using FileSystemManager.Drivers;
using FileSystemManager.ResultTypes;

namespace FileSystemManager.Commands.FileCommands;

public class FileShowCommand : ICommand
{
    private readonly string _path;
    private readonly IDriver _driver;

    public FileShowCommand(string path, IDriver driver)
    {
        _path = path;
        _driver = driver;
    }

    public FileShowCommand(string path) : this(path, new ConsoleDriver()) { }

    public CommandExecutionResult Execute(IContext context)
    {
        if (context.FileSystem is null || context.CurrentPath is null)
            return new CommandExecutionResult.Failure("File System is not connected.");

        string absolutePath = context.FileSystem.GetAbsolutePath(_path, context.CurrentPath);

        try
        {
            StreamReader reader = context.FileSystem.GetContent(absolutePath);

            string? line = reader.ReadLine();
            while (line is not null)
            {
                _driver.Print(line);
                line = reader.ReadLine();
            }
        }
        catch (Exception e)
        {
            return new CommandExecutionResult.Failure(e.Message);
        }

        return new CommandExecutionResult.Success($"File {absolutePath} was shown successfully.");
    }
}