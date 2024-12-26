namespace FileSystemManager.Drivers;

public class ConsoleDriver : IDriver
{
    public void Print(string text)
    {
        Console.WriteLine(text);
    }
}