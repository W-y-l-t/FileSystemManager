using FileSystemManager.Commands;
using FileSystemManager.Commands.FileCommands;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.LowLevelHandlers.FileCommandsHandlers;

public class MoveHandler : AbstractHandler
{
    public override ICommand? Handle(ParsedData arguments)
    {
        return
            arguments.Arguments is not [_, "move", _, _]
                ? base.Handle(arguments)
                : new FileMoveCommand(arguments.Arguments[2], arguments.Arguments[3]);
    }
}