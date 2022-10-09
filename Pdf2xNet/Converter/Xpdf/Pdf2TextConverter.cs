using Pdf2xNet.Infrastructure.Enums;
using Pdf2xNet.Infrastructure.Extensions;
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
    public sealed class Pdf2TextConverter : BaseXpdfConverter<Pdf2Text>
    {
        public Pdf2TextConverter(Pdf2Text options) : base("pdf2text", options)
        {
            if (options.EndOfLine == null)
            {
                var os = PlatformUtility.GetOperatingSystem();

                if (os == OSPlatform.OSX)
                    base.options.EndOfLine = Eol.Mac;
                else if (os == OSPlatform.Linux)
                    base.options.EndOfLine = Eol.Unix;
                else
                    base.options.EndOfLine = Eol.Dos;
            }
        }

        protected override List<string> CreateParameters(Pdf2Text options)
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

        public override async Task<ExitCodes> ExtractAsync([NotNull] string filePath, [NotNull] string outputFile, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(filePath))
                return ExitCodes.Other;

            const string quote = "\"";

            var parameters = CreateParameters(base.options);
            parameters.Add(string.Concat(quote, filePath, quote));
            parameters.Add(string.Concat(quote, outputFile, quote));

            var args = string.Join(" ", parameters);

            return (ExitCodes)await ProcessUtility.Run(base.toolPath, args, base.workingDirectory, cancellationToken);
        }

        public override async Task<List<string>> ExtractAsync([NotNull] string filePath, CancellationToken cancellationToken = default)
        {
            var tempFile = Path.Combine(base.rootTempFolderPath, $"{Path.GetFileNameWithoutExtension(filePath)}-{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}.txt");

            try
            {
                var result = await ExtractAsync(filePath, tempFile, cancellationToken);

                if (result != ExitCodes.NoError)
                    throw new Exception($"Exit Code: [{result}] {result.ToDescriptionString()}");

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
                if(File.Exists(tempFile))
                    File.Delete(tempFile);
                throw;
            }
        }
    }
}