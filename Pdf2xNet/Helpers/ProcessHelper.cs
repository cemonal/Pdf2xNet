using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Pdf2xNet.Helpers
{
    /// <summary>
    /// Helper class for running external processes.
    /// </summary>
    internal static class ProcessHelper
    {
        /// <summary>
        /// Runs an external process asynchronously.
        /// </summary>
        /// <param name="fileName">The path to the executable file.</param>
        /// <param name="arguments">The arguments to pass to the process.</param>
        /// <param name="workingDirectory">The working directory for the process.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the process.</param>
        /// <returns>A task representing the exit code of the process.</returns>
        public static async Task<int> RunAsync(string fileName, string arguments, string workingDirectory, CancellationToken cancellationToken)
        {
            using (var process = CreateNewProcess(fileName, arguments, workingDirectory))
            {
                using (cancellationToken.Register(() =>
                {
                    process.CloseMainWindow(); // Close the main window, if open.
                    process.Kill(); // Kill the process.
                }))
                {
                    var tcs = new TaskCompletionSource<int>();

                    process.EnableRaisingEvents = true;
                    process.Exited += (sender, args) =>
                    {
                        tcs.SetResult(process.ExitCode);
                        process.Dispose();
                    };

                    process.Start();

                    return await tcs.Task.ConfigureAwait(false);
                }
            }
        }

        private static Process CreateNewProcess(string fileName, string arguments, string workingDirectory)
        {
            return new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
        }
    }
}