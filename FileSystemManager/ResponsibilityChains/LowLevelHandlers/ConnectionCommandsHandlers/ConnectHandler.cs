using FileSystemManager.Commands;
using FileSystemManager.Commands.ConnectionCommands.ConnectCommands;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.LowLevelHandlers.ConnectionCommandsHandlers;

public class ConnectHandler : AbstractHandler
{
    public override ICommand? Handle(ParsedData arguments)
    {
        if (arguments.Arguments is not ["connect", _])
            return base.Handle(arguments);

        arguments.Flags.TryGetValue(new EquatableString("-m"), out string? mode);

        return mode switch
        {
            "local" => new ConnectLocalCommand(arguments.Arguments[1]),
            "in-memory" => new ConnectInMemoryCommand(arguments.Arguments[1]),
            _ => base.Handle(arguments),
        };
    }
}