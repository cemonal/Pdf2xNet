using Pdf2xNet.Infrastructure.Enums.Xpdf;
using Pdf2xNet.Infrastructure.Extensions;
using Pdf2xNet.Infrastructure.Models.Xpdf;
using Pdf2xNet.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pdf2xNet.Converter.Xpdf
{
    internal class Pdf2HtmlConverter
    {
        private readonly XpdfUtilities tool;

        public Pdf2HtmlConverter()
        {
            tool = new XpdfUtilities(XpdfTool.Pdf2Html);
        }

        private List<string> CreateParameters(Pdf2Html options)
        {
            var args = new List<string>();

            if (options.FirstPage > 0) args.Add($"-f {options.FirstPage}");
            if (options.LastPage > 0) args.Add($"-l {options.LastPage}");
            if (options.ZoomLevel > 1.0) args.Add($"-z {options.ZoomLevel}");
            if (options.Vstretch > 1.0) args.Add($"-vstretch {options.Vstretch}");
            if (options.Table) args.Add("-table");
            if (options.EmbedFonts) args.Add("-embedfonts");
            if (options.EmbedBackground) args.Add("-embedbackground");
            if (options.FormFields) args.Add("-formfields");
            if (options.SkipInvisible) args.Add("-skipinvisible");

            if (!string.IsNullOrEmpty(options.OwnerPassword))
                args.Add($"-opw {options.OwnerPassword}");

            if (!string.IsNullOrEmpty(options.UserPassword))
                args.Add($"-upw {options.UserPassword}");

            return args;
        }

        public async Task<List<string>> ExtractAsync(Pdf2Html options, [NotNull] string filePath, CancellationToken cancellationToken = default)
        {
            var rootTempFolderPath = tool.GetRootTempFolderPath();
            var tempFolder = Path.Combine(rootTempFolderPath, Guid.NewGuid().ToString());

            try
            {
                await ExtractAsync(options, filePath, tempFolder, cancellationToken);
                
                var pageName = Path.GetFileNameWithoutExtension(filePath);

                var info = new DirectoryInfo(Path.Combine(tempFolder, pageName));
                var files = info.GetFiles($"page*.html").OrderBy(p => p.CreationTime).ToArray();

                var result = new List<string>(files.Count());

                foreach (var file in files)
                {
                    var html = await File.ReadAllTextAsync(file.FullName, Encoding.UTF8, cancellationToken);
                    result.Add(html);
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

        public async Task ExtractAsync(Pdf2Html options, [NotNull] string filePath, [NotNull] string outputPath, CancellationToken cancellationToken = default)
        {
            var result = await tool.RunAsyc(CreateParameters(options), filePath, outputPath, cancellationToken);

            if (result != ExitCodes.NoError)
                throw new Exception($"Exit Code: [{result}] {result.ToDescriptionString()}");
        }
    }
}