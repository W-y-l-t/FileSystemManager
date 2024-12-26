using FileSystemManager.Commands;
using FileSystemManager.ResponsibilityChains;
using FileSystemManager.ValuesObjects;

namespace FileSystemManager.Parsers;

public class Parser : IParser
{
    public Parser()
    {
        Handler = new HighLevelHandler();
    }

    public IHandler Handler { get; }

    public ICommand? ParseCommand(string input)
    {
        return Handler.Handle(ParseData(input));
    }

    private static ParsedData ParseData(string input)
    {
        var rawArguments =
            input.Split(" ").Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();

        var flags = new Dictionary<EquatableString, string>();
        var arguments = new List<string>();

        for (int i = 0; i < rawArguments.Count; i += 1)
        {
            string argument = rawArguments[i];
            if (argument.StartsWith('-'))
            {
                i += 1;
                flags.Add(new EquatableString(argument), rawArguments[i]);
            }
            else
            {
                arguments.Add(argument);
            }
        }

        return new ParsedData(flags, arguments.AsReadOnly());
    }
}