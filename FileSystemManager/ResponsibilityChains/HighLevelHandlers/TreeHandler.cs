using FileSystemManager.Commands;
using FileSystemManager.ResponsibilityChains.LowLevelHandlers.TreeCommandsHandlers;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.HighLevelHandlers;

public class TreeHandler : AbstractHandler
{
    public TreeHandler()
    {
        Handler = new GoToHandler();
        Handler.SetNext(new ListHandler());
    }

    public IHandler Handler { get; }

    public override ICommand? Handle(ParsedData arguments)
    {
        return
            arguments.Arguments is ["tree", ..]
                ? Handler.Handle(arguments)
                : base.Handle(arguments);
    }
}