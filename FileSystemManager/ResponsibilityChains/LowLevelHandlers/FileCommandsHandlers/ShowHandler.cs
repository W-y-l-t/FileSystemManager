using FileSystemManager.Commands;
using FileSystemManager.Commands.FileCommands;
using FileSystemManager.Drivers;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.LowLevelHandlers.FileCommandsHandlers;

public class ShowHandler : AbstractHandler
{
    public override ICommand? Handle(ParsedData arguments)
    {
        if (arguments.Arguments is not [_, "show", _])
            return base.Handle(arguments);

        arguments.Flags.TryGetValue(new EquatableString("-m"), out string? mode);

        return mode switch
        {
            "console" => new FileShowCommand(arguments.Arguments[2], new ConsoleDriver()),
            null => new FileShowCommand(arguments.Arguments[2]),
            _ => base.Handle(arguments),
        };
    }
}