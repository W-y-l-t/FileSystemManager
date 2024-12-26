namespace FileSystemManager.ResultTypes;

public abstract record CommandExecutionResult
{
    private CommandExecutionResult() { }

    public sealed record Failure(string Message) : CommandExecutionResult;

    public sealed record Success(string Message) : CommandExecutionResult;
}