using Pdf2xNet.Infrastructure.Enums;
using Pdf2xNet.Infrastructure.Interfaces.Converters;
using Pdf2xNet.Infrastructure.Interfaces.Models.Xpdf;
using Pdf2xNet.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Pdf2xNet.Converter.Xpdf
{
    public abstract class BaseXpdfConverter<T> : IXpdfConverter where T : IXpdfModel
    {
        protected readonly string programName;
        protected readonly T options;
        protected readonly string toolPath;
        protected readonly string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        protected readonly string rootTempFolderPath;

        public BaseXpdfConverter([NotNull] string programName, T options)
        {
            this.programName = programName;
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.toolPath = XpdfUtility.FindTool(programName);
            this.rootTempFolderPath = Path.Combine(Path.GetTempPath(), programName);

            if (!Directory.Exists(rootTempFolderPath))
                Directory.CreateDirectory(rootTempFolderPath);
        }

        protected virtual List<string> CreateParameters(T options)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<ExitCodes> ExtractAsync([NotNull] byte[] file, [NotNull] string outputPath, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(rootTempFolderPath, $"{Guid.NewGuid()}-{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}.pdf");

            try
            {
                await File.WriteAllBytesAsync(filePath, file, cancellationToken);

                var result = await ExtractAsync(filePath, outputPath, cancellationToken);

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

        public virtual async Task<List<string>> ExtractAsync([NotNull] byte[] file, CancellationToken cancellationToken = default)
        {
            var filePath = Path.Combine(rootTempFolderPath, $"{Guid.NewGuid()}-{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}.pdf");

            try
            {
                await File.WriteAllBytesAsync(filePath, file, cancellationToken);

                var result = await ExtractAsync(filePath, cancellationToken);

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

        public virtual Task<ExitCodes> ExtractAsync([NotNull] string filePath, [NotNull] string outputPath, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<string>> ExtractAsync([NotNull] string filePath, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
