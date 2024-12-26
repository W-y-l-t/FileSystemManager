using FileSystemManager.Commands.ConnectionCommands;
using FileSystemManager.ValuesObjects;
using ICommand = FileSystemManager.Commands.ICommand;

namespace FileSystemManager.ResponsibilityChains.LowLevelHandlers.ConnectionCommandsHandlers;

public class DisconnectHandler : AbstractHandler
{
    public override ICommand? Handle(ParsedData arguments)
    {
        return
            arguments.Arguments is ["disconnect"]
                ? new DisconnectCommand()
                : base.Handle(arguments);
    }
}