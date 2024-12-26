﻿using FileSystemManager.Contexts;
using FileSystemManager.ResultTypes;

namespace FileSystemManager.Commands.FileCommands;

public class FileMoveCommand : ICommand
{
    private readonly string _source;
    private readonly string _destination;

    public FileMoveCommand(string source, string destination)
    {
        _source = source;
        _destination = destination;
    }

    public CommandExecutionResult Execute(IContext context)
    {
        if (context.FileSystem is null || context.CurrentPath is null)
            return new CommandExecutionResult.Failure("File System is not connected.");

        string absoluteSourcePath = context.FileSystem.GetAbsolutePath(_source, context.CurrentPath);

        FileSystemOperationResult moveResult = context.FileSystem.Move(absoluteSourcePath, _destination);

        return moveResult switch
        {
            FileSystemOperationResult.Success success => new CommandExecutionResult.Success(success.Message),
            FileSystemOperationResult.Failure failure => new CommandExecutionResult.Failure(failure.Message),
            _ => new CommandExecutionResult.Failure("Something went wrong."),
        };
    }
}