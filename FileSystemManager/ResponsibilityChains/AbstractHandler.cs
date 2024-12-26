using FileSystemManager.Commands;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains;

public class AbstractHandler : IHandler
{
    private IHandler? _nextHandler;

    public IHandler SetNext(IHandler handler)
    {
        _nextHandler = handler;

        return handler;
    }

    public virtual ICommand? Handle(ParsedData arguments)
    {
        return _nextHandler?.Handle(arguments);
    }
}