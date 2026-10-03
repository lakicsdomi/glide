using System.Diagnostics;
using Glide.Core;

namespace Glide.Commands;

/// <summary>
/// Does the following git workflow:
/// 0. Check if there are any changes to commit
/// 1. git add .
/// 2. git commit -m "commit message"
/// 3. Check upstream branch
/// 4. git push (if upstream branch exists)
/// 5. git push -u origin <branch> (if upstream branch does not exist)
/// </summary>
public class UploadCommand : ICommand
{
    
    public static void Execute(string[] args)
    {

        // Check if commit message is provided
        if (args.Length == 0)
        {
            Console.WriteLine("Error: Commit message is required.");
            Console.WriteLine("Usage: glide upload \"commit message\"");
            return;
        }

        // Get commit message from arguments
        string commitMessage = args[0];

        // Check if there are any changes to commit
        var statusResponse = GitRunner.Run("status --porcelain");
        if (statusResponse.ExitCode != 0)
        {
            Console.WriteLine($"Error checking git status:\n{statusResponse.Error}");
            return;
        }
        if(string.IsNullOrEmpty(statusResponse.Output))
        {
            Console.WriteLine("No changes to commit.");
            return;
        }

        var commands = new List<string>
        {
            "add .",
            $"commit -m \"{commitMessage}\""
            // You can add more commands here as needed
        };

        foreach (var command in commands)
        {
            var response = GitRunner.Run(command);
            if (response.ExitCode != 0)
            {
                string errorMessage = string.IsNullOrWhiteSpace(response.Error) 
                    ? response.Output 
                    : response.Error;
                    
                Console.WriteLine($"Error executing git {command}:\n{errorMessage}");
                return;
            }
            else
            {
                Console.WriteLine($"Successfully executed: git {command}");
            }
        }
    }

    
}