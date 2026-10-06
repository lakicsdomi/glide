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

        if (!HasChanges())
            return;

        if (!StageAll())
            return;

        if (!Commit(args[0]))
            return;

        var branchName = GetBranch();
        if (string.IsNullOrWhiteSpace(branchName))
            return;

        SafePush(branchName);

    }

    /// <summary>
    /// Push to remote branch, create upstream if it doesn't exist.
    /// </summary>
    private static void SafePush(string branch)
    {
        // Check for remote branch
        var response = GitRunner.Run(
            "ls-remote",
            "--exit-code",
            "--heads",
            "origin",
            branch);

        // Upstream branch Exists
        if (response.Success)
        {
            RunCommand("push");
            return;
        }

        // No upstream
        if (response.ExitCode == 2)
        {
            RunCommand("push", "-u", "origin", branch);
            return;
        }

        var errorMessage = string.IsNullOrWhiteSpace(response.Error)
            ? response.Output
            : response.Error;

        Console.WriteLine($"Error checking remote branch:\n{errorMessage}");
    }

    /// <summary>
    /// Get current branch, returns null if error occurs
    /// </summary>
    private static string? GetBranch()
    {
        var response = GitRunner.Run("branch", "--show-current");
        if (!response.Success)
        {
            Console.WriteLine($"Error getting current branch:\n{response.Error}");
            return null;
        }

        return response.Output.Trim();
    }

    /// <summary>
    /// Stage all changes
    /// </summary>
    private static bool StageAll()
    {
        return RunCommand("add", ".");
    }

    /// <summary>
    /// Commit changes
    /// </summary>
    private static bool Commit(string msg)
    {
        return RunCommand("commit", "-m", msg);
    }

    /// <summary>
    /// Check if there are any changes to commit
    /// </summary>
    private static bool HasChanges()
    {
        
        var statusResponse = GitRunner.Run("status", "--porcelain");
        if (!statusResponse.Success)
        {
            Console.WriteLine($"Error checking git status:\n{statusResponse.Error}");
            return false;
        }
        if (string.IsNullOrEmpty(statusResponse.Output))
        {
            Console.WriteLine("No changes to commit.");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Run a git command and handle errors
    /// </summary>
    private static bool RunCommand(params string[] args)
    {
        var response = GitRunner.Run(args);
        if (!response.Success)
        {
            string errorMessage = string.IsNullOrWhiteSpace(response.Error)
                ? response.Output
                : response.Error;

            Console.WriteLine($"Error executing git {String.Join(" ", args)}:\n{errorMessage}");
            return false;
        }
        else
        {
            Console.WriteLine($"Successfully executed: git {String.Join(" ", args)}");
        }
        return true;
    }

    
}