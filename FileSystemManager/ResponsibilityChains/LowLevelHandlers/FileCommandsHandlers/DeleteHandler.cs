using FileSystemManager.Commands;
using FileSystemManager.Commands.FileCommands;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.LowLevelHandlers.FileCommandsHandlers;

public class DeleteHandler : AbstractHandler
{
    public override ICommand? Handle(ParsedData arguments)
    {
        return
            arguments.Arguments is not [_, "delete", _]
                ? base.Handle(arguments)
                : new FileDeleteCommand(arguments.Arguments[2]);
    }
}