using FileSystemManager.Commands;
using FileSystemManager.Contexts;
using FileSystemManager.Drivers;
using FileSystemManager.Parsers;

namespace FileSystemManager;

public class Program
{
    public static void Main()
    {
        var parser = new Parser();
        var context = new Context();
        var driver = new ConsoleDriver();

        Run(parser, context, driver);
    }

    public static void Run(IParser parser, IContext context, IDriver driver)
    {
        while (true)
        {
            driver.Print("> " + context.CurrentPath);

            string? line = Console.ReadLine();

            if (line is null)
                continue;

            ICommand? command = parser.ParseCommand(line);

            var logger = new CommandLoggerDecorator(driver, command);

            logger.Execute(context);
        }
    }
}
