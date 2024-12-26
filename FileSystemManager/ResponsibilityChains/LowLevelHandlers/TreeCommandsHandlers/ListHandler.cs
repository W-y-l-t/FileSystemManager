using FileSystemManager.Commands;
using FileSystemManager.Commands.TreeCommands;
using FileSystemManager.Drivers;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.LowLevelHandlers.TreeCommandsHandlers;

public class ListHandler : AbstractHandler
{
    public override ICommand? Handle(ParsedData arguments)
    {
        if (arguments.Arguments is not [_, "list", ..])
            return base.Handle(arguments);

        arguments.Flags.TryGetValue(new EquatableString("-fm"), out string? fileMark);
        arguments.Flags.TryGetValue(new EquatableString("-dm"), out string? directoryMark);
        arguments.Flags.TryGetValue(new EquatableString("-s"), out string? shift);
        arguments.Flags.TryGetValue(new EquatableString("-d"), out string? depth);
        arguments.Flags.TryGetValue(new EquatableString("-o"), out string? outputMode);

        IDriver driver = outputMode switch
        {
            "console" => new ConsoleDriver(),
            _ => new ConsoleDriver(),
        };

        return
            new TreeListCommand(
                driver,
                new Countable(depth is null ? 1 : int.Parse(depth)),
                directoryMark ?? "[D]",
                fileMark ?? "[F]",
                shift ?? "---");
    }
}