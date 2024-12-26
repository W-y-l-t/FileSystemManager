using FileSystemManager.Commands;
using FileSystemManager.Commands.SystemCommands;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.LowLevelHandlers.SystemCommandsHandler;

public class ExitHandler : AbstractHandler
{
    public override ICommand? Handle(ParsedData arguments)
    {
        return
            arguments.Arguments is ["exit"]
                ? new ExitCommand()
                : base.Handle(arguments);
    }
}