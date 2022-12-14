using Pdf2xNet.Infrastructure.Enums.Xpdf;
using Pdf2xNet.Infrastructure.Extensions;
using Pdf2xNet.Infrastructure.Models.Xpdf;
using Pdf2xNet.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pdf2xNet.Converter.Xpdf
{
    internal class Pdf2PngConverter
    {
        private readonly XpdfUtilities tool;

        public Pdf2PngConverter()
        {
            tool = new XpdfUtilities(XpdfTool.Pdf2Png);
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

        public async Task<List<string>> ExtractAsync(Pdf2Png options, [NotNull] string filePath, CancellationToken cancellationToken = default)
        {
            var rootTempFolderPath = tool.GetRootTempFolderPath();
            var tempFolder = Path.Combine(rootTempFolderPath, Guid.NewGuid().ToString());

            try
            {
                await ExtractAsync(options, filePath, tempFolder, cancellationToken);
 
                var info = new DirectoryInfo(tempFolder);
                var files = info.GetFiles($"*.png").OrderBy(p => p.CreationTime).ToArray();

                var result = new List<string>(files.Count());

                foreach (var file in files)
                {
                    var bytes = await File.ReadAllBytesAsync(file.FullName, cancellationToken);
                    result.Add(Convert.ToBase64String(bytes));
                }

                Directory.Delete(tempFolder);

                return result;
            }
            catch (Exception)
            {
                if (Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder);
                throw;
            }
        }

        public async Task ExtractAsync(Pdf2Png options, [NotNull] string filePath, [NotNull] string outputPath, CancellationToken cancellationToken = default)
        {
            var result = await tool.RunAsyc(CreateParameters(options), filePath, outputPath, cancellationToken);

            if (result != ExitCodes.NoError)
                throw new Exception($"Exit Code: [{result}] {result.ToDescriptionString()}");
        }
    }
}