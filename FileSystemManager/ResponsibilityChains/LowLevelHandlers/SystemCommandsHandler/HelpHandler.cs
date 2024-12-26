using FileSystemManager.Commands;
using FileSystemManager.Commands.SystemCommands;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.LowLevelHandlers.SystemCommandsHandler;

public class HelpHandler : AbstractHandler
{
    public override ICommand? Handle(ParsedData arguments)
    {
        return
            arguments.Arguments is ["help"]
                ? new HelpCommand()
                : base.Handle(arguments);
    }
}