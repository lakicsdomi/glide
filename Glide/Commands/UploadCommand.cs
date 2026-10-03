using System.Diagnostics;

namespace Glide.Commands;

/// <summary>
/// Does the following git workflow:
/// 1. git add .
/// 2. git commit -m "commit message"
/// 3. Check upstream branch
/// 4. git push (if upstream branch exists)
/// 5. git push -u origin <branch> (if upstream branch does not exist)
/// </summary>
public class UploadCommand : ICommand
{
    private class ProcessResponse
    {
        public string Output { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
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

        // Step 1: git add .
        ProcessResponse processResponse = new ProcessResponse();



        var commands = new List<string>
        {
            "add .",
            $"commit -m \"{commitMessage}\""
            // You can add more commands here as needed
        };

        foreach (var command in commands)
        {
            using var process = CreateGitProcess(command);

            process.Start();

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                Console.WriteLine($"Error executing {command}: {processResponse.Error}");
                return;
            }
            else
            {
                Console.WriteLine($"Successfully executed: git {command}");
            }
        }
    }

    private static Process CreateGitProcess(string arguments)
    {
        return new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
    }
}