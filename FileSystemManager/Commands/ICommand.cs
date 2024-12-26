using FileSystemManager.Contexts;
using FileSystemManager.ResultTypes;

namespace FileSystemManager.Commands;

public interface ICommand
{
    CommandExecutionResult Execute(IContext context);
}