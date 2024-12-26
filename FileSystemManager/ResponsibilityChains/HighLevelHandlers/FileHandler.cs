using FileSystemManager.Commands;
using FileSystemManager.ResponsibilityChains.LowLevelHandlers.FileCommandsHandlers;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.HighLevelHandlers;

public class FileHandler : AbstractHandler
{
    public FileHandler()
    {
        Handler = new CopyHandler();
        Handler
            .SetNext(new DeleteHandler())
            .SetNext(new MoveHandler())
            .SetNext(new RenameHandler())
            .SetNext(new ShowHandler());
    }

    public IHandler Handler { get; }

    public override ICommand? Handle(ParsedData arguments)
    {
        return
            arguments.Arguments is ["file", ..]
                ? Handler.Handle(arguments)
                : base.Handle(arguments);
    }
}