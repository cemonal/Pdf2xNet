using Pdf2xNet.Enums.Xpdf;
using Pdf2xNet.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Pdf2xNet.Tools
{
    /// <summary>
    /// Utility class for managing Xpdf tools and performing PDF conversion operations.
    /// </summary>
    internal class XpdfUtilities
    {
        private readonly string toolPath;
        private readonly string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private readonly string rootTempFolderPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="XpdfUtilities"/> class.
        /// </summary>
        /// <param name="tool">The Xpdf tool to use.</param>
        public XpdfUtilities(XpdfTool tool)
        {
            toolPath = FindToolPath(tool.ToString().ToLower());
            rootTempFolderPath = Path.Combine(Path.GetTempPath(), tool.ToString().ToLower());

            if (!Directory.Exists(rootTempFolderPath))
                Directory.CreateDirectory(rootTempFolderPath);
        }

        /// <summary>
        /// Finds the path to the specified Xpdf tool executable.
        /// </summary>
        private string FindToolPath(string tool)
        {
            var os = PlatformHelper.GetOperatingSystem();
            string folder = "Windows";
            string extension = "exe";

            if (os == OSPlatform.Linux)
            {
                folder = "Linux";
                extension = "linux";
            }
            else if (os == OSPlatform.OSX)
            {
                folder = "Mac";
                extension = "mac";
            }

            return Path.Combine("Lib", "Xpdf", folder, Environment.Is64BitOperatingSystem ? "x64" : "x86", $"{tool}.{extension}");
        }

        /// <summary>
        /// Gets the root temporary folder path for Xpdf operations.
        /// </summary>
        public string GetRootTempFolderPath()
        {
            return rootTempFolderPath;
        }

        /// <summary>
        /// Asynchronously runs the specified Xpdf tool with parameters on a PDF file and saves the result to the specified output path.
        /// </summary>
        public async Task<ExitCodes> RunAsyc(List<string> parameters, [NotNull] byte[] file, [NotNull] string outputPath, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(rootTempFolderPath, $"{Guid.NewGuid()}-{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}.pdf");

            try
            {
                await File.WriteAllBytesAsync(filePath, file, cancellationToken).ConfigureAwait(false);

                var result = await RunAsyc(parameters, filePath, outputPath, cancellationToken);

                File.Delete(filePath);

                return result;
            }
            catch (Exception)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                throw;
            }
        }

        /// <summary>
        /// Asynchronously runs the specified Xpdf tool with parameters on a PDF file and saves the result to the specified output path.
        /// </summary>
        public async Task<ExitCodes> RunAsyc(List<string> parameters, [NotNull] string filePath, [NotNull] string outputPath, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(filePath) || !Directory.CreateDirectory(outputPath).Exists)
                return ExitCodes.Other;

            if (outputPath[^1] != Path.DirectorySeparatorChar)
                outputPath = string.Concat(outputPath, Path.DirectorySeparatorChar);

            const string quote = "\"";

            var param = new List<string>();
            param.AddRange(parameters);
            param.Add(string.Concat(quote, filePath, quote));
            param.Add(string.Concat(quote, Path.Combine(outputPath, Path.GetFileNameWithoutExtension(filePath)), quote));

            var args = string.Join(" ", parameters);

            return (ExitCodes)await ProcessHelper.RunAsync(toolPath, args, workingDirectory, cancellationToken).ConfigureAwait(false);
        }
    }
}
