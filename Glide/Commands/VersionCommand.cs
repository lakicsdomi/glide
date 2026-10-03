using System.Reflection;

namespace Glide.Commands;
    
public class VersionCommand : ICommand{
    public static void Execute(){
        var version = Assembly.GetEntryAssembly()?
                            .GetName()
                            .Version;

        if (version == null)
        {
            Console.WriteLine("Version information not available.");
            return;
        }
        Console.WriteLine($"glide v{version.ToString()}");
    }
}