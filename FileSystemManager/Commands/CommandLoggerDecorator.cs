using FileSystemManager.Contexts;
using FileSystemManager.Drivers;
using FileSystemManager.ResultTypes;

namespace FileSystemManager.Commands;

public class CommandLoggerDecorator : ICommand
{
    private readonly IDriver? _driver;
    private readonly ICommand? _command;

    public CommandLoggerDecorator(IDriver? driver, ICommand? command)
    {
        _driver = driver;
        _command = command;
    }

    public CommandLoggerDecorator(ICommand? command) : this(null, command) { }

    public CommandExecutionResult Execute(IContext context)
    {
        string log;

        if (_command is null)
        {
            log = "[Result] Command execution was not started.\n" +
                  "[Reason] Invalid command.";
            _driver?.Print(log);

            return new CommandExecutionResult.Failure(log);
        }

        CommandExecutionResult result = _command.Execute(context);

        string fullCommandType = _command.GetType().ToString();
        string shortCommandType = fullCommandType[(fullCommandType.LastIndexOf('.') + 1)..];

        log = result switch
        {
            CommandExecutionResult.Failure failure => $"[Result] Command {shortCommandType} execution failed.\n" +
                                                      $"[Reason] {failure.Message}",
            CommandExecutionResult.Success success =>
                $"[Result] Command {shortCommandType} execution finished successfully.\n"
                + $"[Message] {success.Message}",
            _ => $"[Result] Command {shortCommandType} execution finished with unexpected result.",
        };

        _driver?.Print(log);

        return new CommandExecutionResult.Success(log);
    }
}