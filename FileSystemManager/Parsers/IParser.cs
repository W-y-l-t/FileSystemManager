using FileSystemManager.Commands;

namespace FileSystemManager.Parsers;

public interface IParser
{
    ICommand? ParseCommand(string input);
}