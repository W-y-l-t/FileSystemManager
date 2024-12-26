using FileSystemManager.Commands;
using FileSystemManager.Commands.TreeCommands;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.LowLevelHandlers.TreeCommandsHandlers;

public class GoToHandler : AbstractHandler
{
    public override ICommand? Handle(ParsedData arguments)
    {
        return
            arguments.Arguments is not [_, "goto", _]
                ? base.Handle(arguments)
                : new TreeGoToCommand(arguments.Arguments[2]);
    }
}