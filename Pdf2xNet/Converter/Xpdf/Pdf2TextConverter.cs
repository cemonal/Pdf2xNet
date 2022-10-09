using Pdf2xNet.Infrastructure.Enums.Xpdf;
using Pdf2xNet.Infrastructure.Extensions;
using Pdf2xNet.Infrastructure.Helpers;
using Pdf2xNet.Infrastructure.Models.Xpdf;
using Pdf2xNet.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Pdf2xNet.Converter.Xpdf
{
    internal class Pdf2TextConverter
    {
        private readonly XpdfUtilities tool;

        public Pdf2TextConverter()
        {
            tool = new XpdfUtilities(XpdfTool.Pdf2Text);
        }

        private List<string> CreateParameters(Pdf2Text options)
        {
            if (options.EndOfLine == null)
            {
                var os = PlatformHelper.GetOperatingSystem();

                if (os == OSPlatform.OSX)
                    options.EndOfLine = Eol.Mac;
                else if (os == OSPlatform.Linux)
                    options.EndOfLine = Eol.Unix;
                else
                    options.EndOfLine = Eol.Dos;
            }

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

        public async Task<List<string>> ExtractAsync(Pdf2Text options, [NotNull] string filePath, CancellationToken cancellationToken = default)
        {
            var rootTempFolderPath = tool.GetRootTempFolderPath();
            var tempFile = Path.Combine(rootTempFolderPath, $"{Path.GetFileNameWithoutExtension(filePath)}-{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}.txt");

            try
            {
                await ExtractAsync(options, filePath, tempFile, cancellationToken);

                string? text = await File.ReadAllTextAsync(tempFile, cancellationToken);

                File.Delete(tempFile);

                var response = new List<string>();

                if (!string.IsNullOrEmpty(text))
                {
                    if (options.NoPageBreaks)
                        response.AddRange(text.Split('\f', StringSplitOptions.RemoveEmptyEntries));
                    else
                        response.Add(text);
                }

                return response;
            }
            catch (Exception)
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
                throw;
            }
        }

        public async Task ExtractAsync(Pdf2Text options, [NotNull] string filePath, [NotNull] string outputPath, CancellationToken cancellationToken = default)
        {
            var result = await tool.RunAsyc(CreateParameters(options), filePath, outputPath, cancellationToken);

            if (result != ExitCodes.NoError)
                throw new Exception($"Exit Code: [{result}] {result.ToDescriptionString()}");
        }
    }
}