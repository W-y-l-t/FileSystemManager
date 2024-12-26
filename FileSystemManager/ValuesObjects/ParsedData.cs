namespace FileSystemManager.ValuesObjects;

public class ParsedData
{
    private readonly Dictionary<EquatableString, string> _flags;
    private readonly List<string> _arguments;

    public ParsedData(IDictionary<EquatableString, string> flags, IReadOnlyCollection<string> arguments)
    {
        _flags = flags.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        _arguments = [.. arguments];
    }

    public IReadOnlyDictionary<EquatableString, string> Flags => _flags;

    public IReadOnlyList<string> Arguments => _arguments;
}