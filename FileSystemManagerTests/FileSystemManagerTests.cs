using FileSystemManager.Commands;
using FileSystemManager.Commands.ConnectionCommands;
using FileSystemManager.Commands.ConnectionCommands.ConnectCommands;
using FileSystemManager.Commands.FileCommands;
using FileSystemManager.Commands.TreeCommands;
using FileSystemManager.Contexts;
using FileSystemManager.FileSystems.InMemoryFileSystems;
using FileSystemManager.Parsers;
using FileSystemManager.ResultTypes;
using Xunit;

namespace Lab4.Tests;

public class Lab4Tests
{
    [Theory]
    [InlineData(@"connect C:\ITMO -m local")]
    [InlineData(@"connect C:\ITMO\DM -m local")]
    [InlineData(@"connect C:\Windows\System32\WindowsPowerShell -m local")]
    public void Parser_ConnectionCommands_CommandTypeIsConnectLocal(string input)
    {
        // Arrange
        var parser = new Parser();

        // Act
        ICommand? command = parser.ParseCommand(input);

        // Assert
        Assert.NotNull(command);
        Assert.IsType<ConnectLocalCommand>(command);
    }

    [Theory]
    [InlineData(@"connect /dir2/subdir1 -m in-memory")]
    [InlineData(@"connect /dir1/subdir2/deepsubdir -m in-memory")]
    [InlineData(@"connect / -m in-memory")]
    public void Parser_ConnectionCommands_CommandTypeIsConnectInMemory(string input)
    {
        // Arrange
        var parser = new Parser();

        // Act
        ICommand? command = parser.ParseCommand(input);

        // Assert
        Assert.NotNull(command);
        Assert.IsType<ConnectInMemoryCommand>(command);
    }

    [Fact]
    public void Parser_ConnectionCommands_CommandTypeIsDisconnect()
    {
        // Arrange
        var parser = new Parser();

        // Act
        ICommand? command = parser.ParseCommand("disconnect");

        // Assert
        Assert.NotNull(command);
        Assert.IsType<DisconnectCommand>(command);
    }

    [Theory]
    [InlineData(@"file copy /dir2/subdir1/file6.txt /file1.txt")]
    [InlineData(@"file copy /dir3 /dir2/subdir2")]
    [InlineData(@"file copy /file3.json /dir2/subdir2/file8.json")]
    public void Parser_FileCommands_CommandTypeIsCopy(string input)
    {
        // Arrange
        var parser = new Parser();

        // Act
        ICommand? command = parser.ParseCommand(input);

        // Assert
        Assert.NotNull(command);
        Assert.IsType<FileCopyCommand>(command);
    }

    [Theory]
    [InlineData(@"file delete /file2.log")]
    [InlineData(@"file delete /dir3/readme.md")]
    [InlineData(@"file delete /dir1/subdir2/deepsubdir/file4.xml")]
    public void Parser_FileCommands_CommandTypeIsDelete(string input)
    {
        // Arrange
        var parser = new Parser();

        // Act
        ICommand? command = parser.ParseCommand(input);

        // Assert
        Assert.NotNull(command);
        Assert.IsType<FileDeleteCommand>(command);
    }

    [Theory]
    [InlineData(@"file move /dir2/subdir1/file6.txt /file1.txt")]
    [InlineData(@"file move /dir3 /dir2/subdir2")]
    [InlineData(@"file move /file3.json /dir2/subdir2/file8.json")]
    public void Parser_FileCommands_CommandTypeIsMove(string input)
    {
        // Arrange
        var parser = new Parser();

        // Act
        ICommand? command = parser.ParseCommand(input);

        // Assert
        Assert.NotNull(command);
        Assert.IsType<FileMoveCommand>(command);
    }

    [Theory]
    [InlineData(@"file rename /file1.txt hello.txt")]
    [InlineData(@"file rename /dir2/subdir2 C#")]
    [InlineData(@"file rename /dir2/subdir2/file8.json cats.json")]
    public void Parser_FileCommands_CommandTypeIsRename(string input)
    {
        // Arrange
        var parser = new Parser();

        // Act
        ICommand? command = parser.ParseCommand(input);

        // Assert
        Assert.NotNull(command);
        Assert.IsType<FileRenameCommand>(command);
    }

    [Theory]
    [InlineData(@"file show /file1.txt -m console")]
    [InlineData(@"file show /dir2/subdir2 -m console")]
    [InlineData(@"file show /dir2/subdir2/file8.json -m console")]
    public void Parser_FileCommands_CommandTypeIsShow(string input)
    {
        // Arrange
        var parser = new Parser();

        // Act
        ICommand? command = parser.ParseCommand(input);

        // Assert
        Assert.NotNull(command);
        Assert.IsType<FileShowCommand>(command);
    }

    [Theory]
    [InlineData(@"tree goto /file2.log")]
    [InlineData(@"tree goto /dir3")]
    [InlineData(@"tree goto /dir1/subdir2/deepsubdir")]
    public void Parser_TreeCommands_CommandTypeIsGoTo(string input)
    {
        // Arrange
        var parser = new Parser();

        // Act
        ICommand? command = parser.ParseCommand(input);

        // Assert
        Assert.NotNull(command);
        Assert.IsType<TreeGoToCommand>(command);
    }

    [Theory]
    [InlineData(@"tree list -d 2")]
    [InlineData(@"tree list -fm [FILE] -dm [DIRECTORY]")]
    [InlineData(@"tree list -o console -d 3 -dm [DDD] -s ---- -fm [FFF]")]
    public void Parser_TreeCommands_CommandTypeList(string input)
    {
        // Arrange
        var parser = new Parser();

        // Act
        ICommand? command = parser.ParseCommand(input);

        // Assert
        Assert.NotNull(command);
        Assert.IsType<TreeListCommand>(command);
    }

    [Theory]
    [InlineData(@"connect / -m file")]
    [InlineData(@"disconnect disconnect")]
    [InlineData(@"file copy /file2.log")]
    [InlineData(@"file rename")]
    [InlineData(@"three list")]
    [InlineData(@"__1_l 239 kk!")]
    public void Parser_Commands_CommandTypeNull(string input)
    {
        // Arrange
        var parser = new Parser();

        // Act
        ICommand? command = parser.ParseCommand(input);

        // Assert
        Assert.Null(command);
    }

    [Fact]
    public void FileSystem_InMemoryFileSystem_Copy()
    {
        // Arrange
        var parser = new Parser();
        var context = new Context();

        ICommand connectCommand =
            parser.ParseCommand("connect / -m in-memory")
            ?? throw new ArgumentNullException($"Parsed command is null");
        connectCommand.Execute(context);

        CreateInMemoryFileSystem(context.FileSystem as InMemoryFileSystem ?? throw new ArgumentNullException());

        ICommand copyCommand =
            parser.ParseCommand("file copy /dir1/subdir2/file3.md /dir1/subdir2/copied_file3.md")
            ?? throw new ArgumentNullException($"Parsed command is null");

        // Act
        CommandExecutionResult result = copyCommand.Execute(context);

        // Assert
        Assert.IsType<InMemoryFileSystem>(context.FileSystem);
        Assert.Contains(
            "/dir1/subdir2/copied_file3.md", context.FileSystem.GetFiles("/dir1/subdir2"));
        Assert.IsType<CommandExecutionResult.Success>(result);
        var revisionResult = result as CommandExecutionResult.Success;
        Assert.Equal("Copied /dir1/subdir2/file3.md to /dir1/subdir2/copied_file3.md", revisionResult?.Message);
    }

    [Fact]
    public void FileSystem_InMemoryFileSystem_Delete()
    {
        // Arrange
        var parser = new Parser();
        var context = new Context();

        ICommand connectCommand =
            parser.ParseCommand("connect / -m in-memory")
            ?? throw new ArgumentNullException($"Parsed command is null");
        connectCommand.Execute(context);

        CreateInMemoryFileSystem(context.FileSystem as InMemoryFileSystem ?? throw new ArgumentNullException());

        ICommand deleteCommand =
            parser.ParseCommand("file delete /dir1/subdir2/deepsubdir/file4.xml")
            ?? throw new ArgumentNullException($"Parsed command is null");

        // Act
        CommandExecutionResult result = deleteCommand.Execute(context);

        // Assert
        Assert.IsType<InMemoryFileSystem>(context.FileSystem);
        Assert.DoesNotContain(
            "/dir1/subdir2/deepsubdir/file4.xml",
            context.FileSystem.GetFiles("/dir1/subdir2/deepsubdir"));
        Assert.IsType<CommandExecutionResult.Success>(result);
        var revisionResult = result as CommandExecutionResult.Success;
        Assert.Equal("Deleted /dir1/subdir2/deepsubdir/file4.xml", revisionResult?.Message);
    }

    [Fact]
    public void FileSystem_InMemoryFileSystem_Move()
    {
        // Arrange
        var parser = new Parser();
        var context = new Context();

        ICommand connectCommand =
            parser.ParseCommand("connect / -m in-memory")
            ?? throw new ArgumentNullException($"Parsed command is null");
        connectCommand.Execute(context);

        CreateInMemoryFileSystem(context.FileSystem as InMemoryFileSystem ?? throw new ArgumentNullException());

        ICommand moveCommand
            = parser.ParseCommand("file move /dir2/subdir1/file6.txt /dir3/file6_moved.txt")
              ?? throw new ArgumentNullException($"Parsed command is null");

        // Act
        CommandExecutionResult result = moveCommand.Execute(context);

        // Assert
        Assert.IsType<InMemoryFileSystem>(context.FileSystem);
        Assert.DoesNotContain(
            "/dir2/subdir1/file6.txt", context.FileSystem.GetFiles("/dir2/subdir1"));
        Assert.Contains("/dir3/file6_moved.txt", context.FileSystem.GetFiles("/dir3"));
        Assert.IsType<CommandExecutionResult.Success>(result);
        var revisionResult = result as CommandExecutionResult.Success;
        Assert.Equal("Moved /dir2/subdir1/file6.txt to /dir3/file6_moved.txt", revisionResult?.Message);
    }

    [Fact]
    public void FileSystem_InMemoryFileSystem_Rename()
    {
        // Arrange
        var parser = new Parser();
        var context = new Context();

        ICommand connectCommand =
            parser.ParseCommand("connect / -m in-memory")
            ?? throw new ArgumentNullException($"Parsed command is null");
        connectCommand.Execute(context);

        CreateInMemoryFileSystem(context.FileSystem as InMemoryFileSystem ?? throw new ArgumentNullException());

        ICommand renameCommand =
            parser.ParseCommand("file rename /file2.log renamed_file2.log")
            ?? throw new ArgumentNullException($"Parsed command is null");

        // Act
        CommandExecutionResult result = renameCommand.Execute(context);

        // Assert
        Assert.IsType<InMemoryFileSystem>(context.FileSystem);
        Assert.DoesNotContain("/file2.log", context.FileSystem.GetFiles("/"));
        Assert.Contains("/renamed_file2.log", context.FileSystem.GetFiles("/"));
        Assert.IsType<CommandExecutionResult.Success>(result);
        var revisionResult = result as CommandExecutionResult.Success;
        Assert.Equal("File /file2.log renamed to /renamed_file2.log", revisionResult?.Message);
    }

    [Fact]
    public void FileSystem_InMemoryFileSystem_Show()
    {
        // Arrange
        var parser = new Parser();
        var context = new Context();

        ICommand connectCommand =
            parser.ParseCommand("connect / -m in-memory")
            ?? throw new ArgumentNullException($"Parsed command is null");
        connectCommand.Execute(context);

        CreateInMemoryFileSystem(context.FileSystem as InMemoryFileSystem ?? throw new ArgumentNullException());

        ICommand showCommand =
            parser.ParseCommand("file show /dir2/subdir1/nested1/file7.html")
            ?? throw new ArgumentNullException($"Parsed command is null");

        // Act
        CommandExecutionResult result = showCommand.Execute(context);

        // Assert
        Assert.IsType<InMemoryFileSystem>(context.FileSystem);
        Assert.IsType<CommandExecutionResult.Success>(result);
        var revisionResult = result as CommandExecutionResult.Success;
        Assert.Equal("File /dir2/subdir1/nested1/file7.html was shown successfully.", revisionResult?.Message);
    }

    private static void CreateInMemoryFileSystem(InMemoryFileSystem fileSystem)
    {
        fileSystem.CreateNode("/", FileSystemNodeType.Directory);

        fileSystem.CreateNode("/dir1", FileSystemNodeType.Directory);
        fileSystem.CreateNode("/dir2", FileSystemNodeType.Directory);
        fileSystem.CreateNode("/dir3", FileSystemNodeType.Directory);

        fileSystem.CreateNode("/dir1/subdir1", FileSystemNodeType.Directory);
        fileSystem.CreateNode("/dir1/subdir2", FileSystemNodeType.Directory);
        fileSystem.CreateNode("/dir1/subdir2/deepsubdir", FileSystemNodeType.Directory);

        fileSystem.CreateNode("/dir2/subdir1", FileSystemNodeType.Directory);
        fileSystem.CreateNode("/dir2/subdir1/nested1", FileSystemNodeType.Directory);
        fileSystem.CreateNode("/dir2/subdir2", FileSystemNodeType.Directory);

        fileSystem.CreateNode("/file1.txt", FileSystemNodeType.File, "Content of file1 in root.");
        fileSystem.CreateNode("/file2.log", FileSystemNodeType.File, "Content of file2 in root.");
        fileSystem.CreateNode("/file3.json", FileSystemNodeType.File, "{\"key\": \"value\"}");

        fileSystem.CreateNode(
            "/dir1/file1.txt", FileSystemNodeType.File, "Content of file1 in dir1.");
        fileSystem.CreateNode(
            "/dir1/subdir1/file2.txt", FileSystemNodeType.File, "Content of file2 in dir1/subdir1.");
        fileSystem.CreateNode(
            "/dir1/subdir2/file3.md", FileSystemNodeType.File, "# Markdown File\nContent of file3.");
        fileSystem.CreateNode(
            "/dir1/subdir2/deepsubdir/file4.xml",
            FileSystemNodeType.File,
            "<root><data>Deep content</data></root>");

        fileSystem.CreateNode(
            "/dir2/subdir1/file6.txt", FileSystemNodeType.File, "Content of file6 in dir2/subdir1.");
        fileSystem.CreateNode(
            "/dir2/subdir1/nested1/file7.html",
            FileSystemNodeType.File,
            "<html><body>Hello, World!</body></html>");
        fileSystem.CreateNode(
            "/dir2/subdir2/file8.json", FileSystemNodeType.File, "{\"another_key\": \"another_value\"}");

        fileSystem.CreateNode(
            "/dir3/readme.md", FileSystemNodeType.File, "# Readme\nWelcome to dir3.");
        fileSystem.CreateNode(
            "/dir3/config.ini", FileSystemNodeType.File, "[settings]\nkey=value");
    }
}