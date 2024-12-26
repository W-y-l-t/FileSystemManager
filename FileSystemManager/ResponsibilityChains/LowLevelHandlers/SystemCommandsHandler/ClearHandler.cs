using FileSystemManager.Commands;
using FileSystemManager.Commands.SystemCommands;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.LowLevelHandlers.SystemCommandsHandler;

public class ClearHandler : AbstractHandler
{
    public override ICommand? Handle(ParsedData arguments)
    {
        return
            arguments.Arguments is ["clear"]
                ? new ClearCommand()
                : base.Handle(arguments);
    }
}