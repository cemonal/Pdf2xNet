using Pdf2xNet.Infrastructure.Models.Xpdf;
using Pdf2xNet.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Pdf2xNet.Infrastructure.Interfaces.Converters;
using Pdf2xNet.Infrastructure.Enums;
using Pdf2xNet.Infrastructure.Extensions;

namespace Pdf2xNet.Converter.Xpdf
{
    public class Pdf2PngConverter : IXpdfConverter
    {
        private readonly Pdf2Png _options;

        public Pdf2PngConverter(Pdf2Png options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        private List<string> CreateParameters(Pdf2Png options)
        {
            var args = new List<string>();

            if (options.FirstPage > 0) args.Add($"-f {options.FirstPage}");
            if (options.LastPage > 0) args.Add($"-l {options.LastPage}");
            if (options.Mono) args.Add("-mono");
            if (options.Gray) args.Add("-gray");
            if (options.Alpha) args.Add("-alpha");
            if (options.Resolution != 150) args.Add($"-r {options.Resolution}");

            args.Add($"-rot {options.Rotate}");

            if (options.FreeType) args.Add("-freetype yes");
            if (options.FontAntiAliasing) args.Add("-aa yes");
            if (options.VectorAntiAliasing) args.Add("-aaVector yes");

            if (!string.IsNullOrEmpty(options.OwnerPassword))
                args.Add($"-opw {options.OwnerPassword}");

            if (!string.IsNullOrEmpty(options.UserPassword))
                args.Add($"-upw {options.UserPassword}");

            return args;
        }

        public async Task<ExitCodes> ExtractAsync([NotNull] string filePath, [NotNull] string outputDirectory, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(filePath) || !Directory.CreateDirectory(outputDirectory).Exists)
                return ExitCodes.Other;

            if (outputDirectory[^1] != Path.DirectorySeparatorChar)
                outputDirectory = string.Concat(outputDirectory, Path.DirectorySeparatorChar);

            const string quote = "\"";

            var parameters = CreateParameters(_options);
            parameters.Add(string.Concat(quote, filePath, quote));
            parameters.Add(string.Concat(quote, Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(filePath)), quote));

            var toolPath = XpdfUtility.FindPdf2PngTool();
            var args = string.Join(" ", parameters);

            return (ExitCodes)await ProcessUtility.Run(toolPath, args, AppDomain.CurrentDomain.BaseDirectory, cancellationToken);
        }

        public async Task<List<string>> ExtractAsync([NotNull] string filePath, CancellationToken cancellationToken = default)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), "pdf2png");

            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            tempFolder = Path.Combine(tempFolder, Guid.NewGuid().ToString());

            var result = await ExtractAsync(filePath, tempFolder, cancellationToken);

            if (result != ExitCodes.NoError)
                throw new Exception($"Exit Code: [{result}] {result.ToDescriptionString()}");

            var files = Directory.GetFiles(tempFolder, "*.png");

            var convertedResult = new List<string>(files.Count());

            foreach (var file in files)
            {
                var bytes = await File.ReadAllBytesAsync(file, cancellationToken);
                convertedResult.Add(Convert.ToBase64String(bytes));
                File.Delete(file);
            }

            Directory.Delete(tempFolder);

            return convertedResult;
        }
    }
}