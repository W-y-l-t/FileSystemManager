using FileSystemManager.Commands;
using FileSystemManager.Commands.FileCommands;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.LowLevelHandlers.FileCommandsHandlers;

public class CopyHandler : AbstractHandler
{
    public override ICommand? Handle(ParsedData arguments)
    {
        return
            arguments.Arguments is not [_, "copy", _, _]
            ? base.Handle(arguments)
            : new FileCopyCommand(arguments.Arguments[2], arguments.Arguments[3]);
    }
}