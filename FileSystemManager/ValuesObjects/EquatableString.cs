namespace FileSystemManager.ValuesObjects;

public class EquatableString : IEquatable<EquatableString>
{
    public EquatableString(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public bool Equals(EquatableString? other)
    {
        return other is not null
               && (ReferenceEquals(this, other)
                   || Value == other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is not null
               && (ReferenceEquals(this, obj)
                                   || (obj.GetType() == GetType() && Equals((EquatableString)obj)));
    }

    public override int GetHashCode()
    {
        return string.GetHashCode(Value, StringComparison.Ordinal);
    }
}