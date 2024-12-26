using FileSystemManager.Contexts;
using FileSystemManager.Drivers;
using FileSystemManager.ResultTypes;

namespace FileSystemManager.Commands.SystemCommands;

public class HelpCommand : ICommand
{
    private const string Commands = """
                                    
                                     --- CONNECTION COMMANDS ---
                                    
                                     connect [Address] [-m Mode]
                                        Address - Absolute path in the file system to connect to.
                                        Mode - File system mode (values: local, in-memory).
                                    
                                     disconnect
                                        Disconnects from the file system.
                                    
                                     --- FILE COMMANDS ---
                                    
                                     file show [Path] {-m Mode}
                                        Path - Relative or absolute path to a file.
                                        Mode - File output mode (value: console).
                                    
                                     file move [SourcePath] [DestinationPath]
                                        SourcePath - Relative or absolute path to the file to be moved.
                                        DestinationPath - Relative or absolute path to the directory where the file should be moved.
                                    
                                     file copy [SourcePath] [DestinationPath]
                                        SourcePath - Relative or absolute path to the file to be copied.
                                        DestinationPath - Relative or absolute path to the directory where the file should be copied.
                                    
                                     file delete [Path]
                                        Path - Relative or absolute path to the file to be deleted.
                                    
                                     file rename [Path] [Name]
                                        Path - Relative or absolute path to the file to be renamed.
                                        Name - New name for the file.
                                    
                                     --- TREE COMMANDS ---
                                    
                                     tree goto [Path]
                                        Path - Relative or absolute path to a directory in the file system.
                                    
                                     tree list {-d Depth} {-fm FileMark} {-dm DirectoryMark} {-o OutputMode} {-s Shift}
                                        Depth - A parameter defining the depth of the listing (default: 1).
                                        FileMark - A parameter defining the sequence of characters that will be used to designate files.
                                        DirectoryMark - A parameter defining the sequence of characters that will be used to designate folders.
                                        OutputMode - A parameter that determines where the file tree will be displayed (value: console, default: console).
                                        Shift - A parameter that defines the sequence of characters to indent (to show the hierarchy).
                                    
                                     --- SYSTEM COMMANDS ---
                                    
                                     exit
                                        Exits the application.
                                    
                                     help {-o OutputMode}
                                        OutputMode - A parameter that determines where information be displayed (value: console, default: console).
                                        Shows supported commands.
                                        
                                     clear
                                        Clears the output in the console.

                                    """;

    private readonly IDriver _driver;

    public HelpCommand(IDriver driver)
    {
        _driver = driver;
    }

    public HelpCommand() : this(new ConsoleDriver()) { }

    public CommandExecutionResult Execute(IContext context)
    {
        _driver.Print(Commands);

        return new CommandExecutionResult.Success("The commands and their descriptions were shown.");
    }
}
