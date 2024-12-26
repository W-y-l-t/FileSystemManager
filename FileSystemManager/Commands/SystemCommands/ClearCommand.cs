using FileSystemManager.Contexts;
using FileSystemManager.ResultTypes;

namespace FileSystemManager.Commands.SystemCommands;

public class ClearCommand : ICommand
{
    public CommandExecutionResult Execute(IContext context)
    {
        Console.Write("\f\u001bc\x1b[3J");

        return new CommandExecutionResult.Success($"Successfully cleared console output.");
    }
}