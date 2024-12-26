using FileSystemManager.Commands;
using FileSystemManager.ResponsibilityChains.LowLevelHandlers.ConnectionCommandsHandlers;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains.HighLevelHandlers;

public class ConnectionHandler : AbstractHandler
{
    public ConnectionHandler()
    {
        Handler = new ConnectHandler();
        Handler.SetNext(new DisconnectHandler());
    }

    public IHandler Handler { get; }

    public override ICommand? Handle(ParsedData arguments)
    {
        return
            arguments.Arguments is ["connect", ..] or ["disconnect", ..]
                ? Handler.Handle(arguments)
                : base.Handle(arguments);
    }
}