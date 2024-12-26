using FileSystemManager.Commands;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.ResponsibilityChains;

public interface IHandler
{
    IHandler SetNext(IHandler handler);

    ICommand? Handle(ParsedData arguments);
}