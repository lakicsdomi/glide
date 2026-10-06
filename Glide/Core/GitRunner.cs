using System.Diagnostics;

namespace Glide.Core;

public class GitRunner
{
    public class ProcessResponse
    {
        public int ExitCode { get; set; }
        public string Output { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public bool Success => ExitCode == 0;
    }

    public static ProcessResponse Run(params string[] arguments)
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

    private static Process CreateGitProcess(string[] arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "git",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        return new Process { StartInfo = startInfo };
    }
}