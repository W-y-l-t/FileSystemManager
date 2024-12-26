namespace FileSystemManager.ValuesObjects;

public class Countable
{
    private int _value;

    public Countable(int value)
    {
        Value = value;
    }

    public int Value
    {
        get => _value;
        set
        {
            _value = value;

            if (_value < 0)
                throw new ArgumentException("Value cannot be negative");
        }
    }
}