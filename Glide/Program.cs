using Glide.Commands;

namespace Glide
{
    public class Program
    {
        // Entry point for the Glide application
        public static void Main(string[] args)
        {
            
            
            

            // Get command from argument
            string command = args.Length > 0 ? args[0] : "help";
            switch(command.ToLower())
            {
                case "help":
                    HelpCommand.Execute();
                    break;
                case "version":
                    VersionCommand.Execute();
                    break;
                case "upload":
                    UploadCommand.Execute(args.Skip(1).ToArray());
                    break;
                default:
                    Console.WriteLine($"Unknown command: {command}");
                    HelpCommand.Execute();
                    break;
            }
            



        }
    }
}