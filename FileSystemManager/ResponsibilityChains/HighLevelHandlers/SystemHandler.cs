using FileSystemManager.Commands;
using FileSystemManager.ResponsibilityChains.LowLevelHandlers.SystemCommandsHandler;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.HighLevelHandlers;

public class SystemHandler : AbstractHandler
{
    public SystemHandler()
    {
        Handler = new ExitHandler();
        Handler
            .SetNext(new HelpHandler())
            .SetNext(new ClearHandler());
    }

    public IHandler Handler { get; }

    public override ICommand? Handle(ParsedData arguments)
    {
        return
            arguments.Arguments is ["exit"] or ["help", ..] or ["clear"]
                ? Handler.Handle(arguments)
                : base.Handle(arguments);
    }
}