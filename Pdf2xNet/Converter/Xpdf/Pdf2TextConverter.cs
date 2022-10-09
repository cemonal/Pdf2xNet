using Pdf2xNet.Infrastructure.Enums;
using Pdf2xNet.Infrastructure.Extensions;
using Pdf2xNet.Infrastructure.Interfaces.Converters;
using Pdf2xNet.Infrastructure.Models.Xpdf;
using Pdf2xNet.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Pdf2xNet.Converter.Xpdf
{
    public sealed class Pdf2TextConverter : IXpdfConverter
    {
        private readonly Pdf2Text _options;

        public Pdf2TextConverter(Pdf2Text options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));

            if (options.EndOfLine == null)
            {
                var os = PlatformUtility.GetOperatingSystem();

                if (os == OSPlatform.OSX)
                    options.EndOfLine = Eol.Mac;
                else if (os == OSPlatform.Linux)
                    options.EndOfLine = Eol.Unix;
                else
                    options.EndOfLine = Eol.Dos;
            }
        }

        private List<string> CreateParameters(Pdf2Text options)
        {
            var args = new List<string>();

            if (options.FirstPage > 0) args.Add($"-f {options.FirstPage}");
            if (options.LastPage > 0) args.Add($"-l {options.LastPage}");
            if (options.Layout) args.Add("-layout");
            if (options.Simple) args.Add("-simple");
            if (options.Simple2) args.Add("-simple2");
            if (options.Table) args.Add("-table");
            if (options.LinePrinter) args.Add("-lineprinter");
            if (options.Raw) args.Add("-raw");

            args.Add($"-enc {options.EncodingName}");
            args.Add($"-eol {options.EndOfLine.ToString().ToLower()}");

            if (options.NoDiagonal) args.Add("-nodiag");
            if (options.Clip) args.Add("-clip");
            if (options.NoPageBreaks) args.Add("-nopgbrk");

            if (!string.IsNullOrEmpty(options.OwnerPassword))
                args.Add($"-opw {options.OwnerPassword}");

            if (!string.IsNullOrEmpty(options.UserPassword))
                args.Add($"-upw {options.UserPassword}");

            return args;
        }

        public async Task<ExitCodes> ExtractAsync([NotNull] byte[] file, [NotNull] string outputFile, CancellationToken cancellationToken = default)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), "pdf2text");

            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            var filePath = Path.Combine(tempFolder, $"{Guid.NewGuid()}-{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}.pdf");

            await File.WriteAllBytesAsync(filePath, file, cancellationToken);

            var result = await ExtractAsync(filePath, outputFile, cancellationToken);

            File.Delete(filePath);

            return result;
        }

        public async Task<ExitCodes> ExtractAsync([NotNull] string filePath, [NotNull] string outputFile, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(filePath))
                return ExitCodes.Other;

            const string quote = "\"";

            var parameters = CreateParameters(_options);
            parameters.Add(string.Concat(quote, filePath, quote));
            parameters.Add(string.Concat(quote, outputFile, quote));

            var toolPath = XpdfUtility.FindPdf2TextTool();
            var args = string.Join(" ", parameters);

            return (ExitCodes)await ProcessUtility.Run(toolPath, args, AppDomain.CurrentDomain.BaseDirectory, cancellationToken);
        }

        public async Task<List<string>> ExtractAsync(byte[] file, CancellationToken cancellationToken = default)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), "pdf2text");

            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            var filePath = Path.Combine(tempFolder, $"{Guid.NewGuid()}-{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}.pdf");

            await File.WriteAllBytesAsync(filePath, file, cancellationToken);

            var result = await ExtractAsync(filePath, cancellationToken);

            File.Delete(filePath);

            return result;
        }

        public async Task<List<string>> ExtractAsync([NotNull] string filePath, CancellationToken cancellationToken = default)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), "pdf2text");

            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            var tempFile = Path.Combine(tempFolder, $"{Path.GetFileNameWithoutExtension(filePath)}-{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}.txt");

            if (File.Exists(tempFile))
                File.Delete(tempFile);

            var result = await ExtractAsync(filePath, tempFile, cancellationToken);

            if (result != ExitCodes.NoError)
                throw new Exception($"Exit Code: [{result}] {result.ToDescriptionString()}");

            string? text = await File.ReadAllTextAsync(tempFile, cancellationToken);

            File.Delete(tempFile);

            var response = new List<string>();

            if (!string.IsNullOrEmpty(text))
                response.AddRange(text.Split('\f', StringSplitOptions.RemoveEmptyEntries));

            return response;
        }
    }
}