using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Pdf2xNet.Infrastructure.Helpers
{
    internal static class ProcessHelper
    {
        public static Task<int> Run(string fileName, string arguments, string workingDirectory, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<int>();
            var process = CreateNewProcess(fileName, arguments, workingDirectory);

            using (cancellationToken.Register(() =>
            {

                tcs.TrySetCanceled();
            }))
            {
                process.Exited += (sender, args) =>
                {
                    tcs.SetResult(process.ExitCode);
                    process.Dispose();
                };

                process.Start();
                return tcs.Task;
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
                },
                EnableRaisingEvents = true
            };
        }
    }
}