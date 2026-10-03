namespace Glide.Commands;

public class HelpCommand : ICommand
{
    public static void Execute()
    {
        Console.WriteLine("Usage: glide [command]");
        Console.WriteLine();
        Console.WriteLine("Commands:");
        Console.WriteLine("  help      Show this help message");
        Console.WriteLine("  version   Show the version of Glide");
    }
}