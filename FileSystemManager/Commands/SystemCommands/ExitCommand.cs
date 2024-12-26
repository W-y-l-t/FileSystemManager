using FileSystemManager.Contexts;
using FileSystemManager.ResultTypes;

namespace FileSystemManager.Commands.SystemCommands;

public class ExitCommand : ICommand
{
    public CommandExecutionResult Execute(IContext context)
    {
        Environment.Exit(0);

        return new CommandExecutionResult.Success($"Successfully exited from program.");
    }
}