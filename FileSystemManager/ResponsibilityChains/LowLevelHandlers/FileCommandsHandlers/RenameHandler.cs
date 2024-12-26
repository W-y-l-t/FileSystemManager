using FileSystemManager.Commands;
using FileSystemManager.Commands.FileCommands;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.LowLevelHandlers.FileCommandsHandlers;

public class RenameHandler : AbstractHandler
{
    public override ICommand? Handle(ParsedData arguments)
    {
        return
            arguments.Arguments is not [_, "rename", _, _]
                ? base.Handle(arguments)
                : new FileRenameCommand(arguments.Arguments[2], arguments.Arguments[3]);
    }
}