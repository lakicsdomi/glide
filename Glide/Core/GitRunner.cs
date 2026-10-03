using System.Diagnostics;

namespace Glide.Core;

public class GitRunner
{
    public class ProcessResponse
    {
        public int ExitCode { get; set; }
        public string Output { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }

    public static ProcessResponse Run(string arguments)
    {
        ProcessResponse processResponse = new ProcessResponse();
        using var process = CreateGitProcess(arguments);
        process.Start();
        processResponse.Output = process.StandardOutput.ReadToEnd();
        processResponse.Error = process.StandardError.ReadToEnd();
        process.WaitForExit();
        processResponse.ExitCode = process.ExitCode;
        return processResponse;    
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