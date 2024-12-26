namespace FileSystemManager.ResultTypes;

public abstract record FileSystemOperationResult
{
    private FileSystemOperationResult() { }

    public sealed record Failure(string Message) : FileSystemOperationResult;

    public sealed record Success(string Message) : FileSystemOperationResult;
}